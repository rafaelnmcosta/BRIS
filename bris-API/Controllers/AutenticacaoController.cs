using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using bris_API.Data;
using bris_API.Models;
using bris_API.Services;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    /// <summary>
    /// Controller responsável pela autenticação, cadastro, login, logout, recuperação de senha e verificação do status de autenticação.
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class AutenticacaoController : ControllerBase, IAutenticacaoController
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;

        /// <summary>
        /// Construtor que injeta as dependências necessárias.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        /// <param name="tokenService">Serviço para geração e manipulação de tokens.</param>
        /// <param name="passwordService">Serviço para manipulação de senhas (hash, salt, etc.).</param>
        /// <param name="emailService">Serviço para envio de emails.</param>
        public AutenticacaoController(AppDbContext context, ITokenService tokenService, IPasswordService passwordService, IEmailService emailService)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _emailService = emailService;
        }

        /// <summary>
        /// Endpoint para cadastro de um novo usuário.
        /// Cria um novo registro de usuário, gera a senha com hash e salt, e cria um vínculo com a role "PENDENTE".
        /// </summary>
        /// <param name="modelUsuario">Dados para cadastro do usuário.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpPost("cadastro")]
        public async Task<IActionResult> Cadastro([FromBody] AutoCadastroDTO modelUsuario)
        {
            try
            {
                // Verifica se já existe um usuário com o mesmo email
                if (await _context.Usuarios.AnyAsync(u => u.Email == modelUsuario.Email))
                    return BadRequest("Já existe um usuário com esse email!");

                // Cria o objeto de usuário com os dados fornecidos
                var usuario = new Usuario
                {
                    Nome = modelUsuario.Nome,
                    Email = modelUsuario.Email,
                    CPF = modelUsuario.CPF,
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Gera salt e hash para a senha fornecida
                var salt = _passwordService.GenerateSalt();
                var hash = _passwordService.HashPassword(modelUsuario.Senha, salt);

                // Cria o registro de senha para o usuário
                var senha = new Senha
                {
                    UsuarioId = usuario.Id,
                    SenhaHash = hash,
                    Salt = salt
                };
                _context.Senhas.Add(senha);

                // Cria um vínculo inicial com a role "PENDENTE" (RoleId 98)
                var novoAcesso = new Vinculo
                {
                    UsuarioId = usuario.Id,
                    GranjaId = null,
                    AgroindustriaId = null,
                    RoleId = 98
                };
                _context.Vinculos.Add(novoAcesso);

                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuário registrado com sucesso! (Conta precisa de ativação)" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao realizar cadastro: " + ex.Message);
            }
        }

        /// <summary>
        /// Endpoint para login de usuário.
        /// Verifica as credenciais do usuário e, se corretas, gera um token JWT e configura um cookie HTTP-Only com o token.
        /// </summary>
        /// <param name="modelLogin">Dados de login (email e senha).</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto modelLogin)
        {
            try
            {
                // Busca o usuário pelo email, incluindo o registro de senha
                var usuario = await _context.Usuarios
                    .Include(u => u.Senha)
                    .FirstOrDefaultAsync(u => u.Email == modelLogin.Email);

                // Verifica se o usuário foi encontrado e se a senha é válida
                if (usuario == null || !_passwordService.VerifyPassword(modelLogin.Senha, usuario.Senha.Salt, usuario.Senha.SenhaHash))
                {
                    return Unauthorized();
                }

                // Obtém informações da requisição para incluir no token
                var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = Request.Headers["User-Agent"].ToString();

                // Gera o token JWT com o ID do usuário e os dados da requisição
                var token = _tokenService.GenerateTokenLogin(usuario.Id.ToString(), userIp, userAgent);

                // Configura o cookie HTTP-Only para o token gerado
                _tokenService.SetCookieToken(HttpContext, token);

                return Ok(new { message = "Login efetuado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao efetuar login: " + ex.Message);
            }
        }

        /// <summary>
        /// Endpoint para logout.
        /// Remove o cookie de autenticação, efetivamente deslogando o usuário.
        /// </summary>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            try
            {
                HttpContext.Response.Cookies.Delete("auth_token");
                return Ok(new { message = "Logout efetuado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao efetuar logout: " + ex.Message);
            }
        }

        /// <summary>
        /// Endpoint que retorna a lista de vínculos do usuário autenticado.
        /// A resposta varia conforme se o token é de acesso (AcessoLogin) ou de vínculo.
        /// </summary>
        /// <returns>Lista de vínculos no formato GetVinculoDTO.</returns>
        [Authorize(Policy = "AcessoLoginOuTodosUsuarios")]
        [HttpGet("vinculos")]
        public async Task<IActionResult> GetVinculos()
        {
            try
            {
                Console.WriteLine("Entrou na getVinculos");

                // Obtém o NameIdentifier do token (pode ser o ID do usuário ou do vínculo)
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Token inválido. (Id na claim do token é null)");
                }

                // Verifica se o token possui a claim "AcessoLogin" com valor "true"
                var isAcessoLogin = User.HasClaim("AcessoLogin", "true");

                if (isAcessoLogin)
                {
                    // Se for token de acesso, o NameIdentifier é o ID do usuário.
                    Console.WriteLine("Token de acesso detectado.");
                    var usuarioId = int.Parse(userIdClaim);

                    // Busca os vínculos associados ao usuário, incluindo as entidades relacionadas.
                    var vinculos = await _context.Vinculos
                        .Where(v => v.UsuarioId == usuarioId)
                        .Include(v => v.Granja)
                        .Include(v => v.Agroindustria)
                        .Include(v => v.Role)
                        .ToListAsync();

                    if (!vinculos.Any())
                    {
                        return NotFound("Nenhum vínculo encontrado para este usuário.");
                    }

                    // Mapeia os vínculos para DTOs
                    var vinculosDTOS = vinculos.Select(v => new GetVinculoDTO
                    {
                        Id = v.Id,
                        Role = v.Role?.Nome ?? "!!! Role não definida !!!",
                        NomeGranja = v.Granja?.NomePropriedade,
                        NomeAgroindustria = v.Agroindustria?.NomeFantasia
                    }).ToList();

                    return Ok(vinculosDTOS);
                }
                else
                {
                    // Se for token de vínculo, o NameIdentifier é o ID do vínculo.
                    Console.WriteLine("Token de vínculo detectado.");
                    var vinculoId = int.Parse(userIdClaim);

                    // Busca o vínculo atual para obter o ID do usuário associado
                    var vinculoAtual = await _context.Vinculos
                        .FirstOrDefaultAsync(v => v.Id == vinculoId);

                    if (vinculoAtual == null)
                    {
                        return NotFound("Vínculo atual não encontrado.");
                    }

                    // Busca todos os vínculos do mesmo usuário
                    var usuarioId = vinculoAtual.UsuarioId;
                    var vinculos = await _context.Vinculos
                        .Where(v => v.UsuarioId == usuarioId)
                        .Include(v => v.Granja)
                        .Include(v => v.Agroindustria)
                        .Include(v => v.Role)
                        .ToListAsync();

                    if (!vinculos.Any())
                    {
                        return NotFound("Nenhum outro vínculo encontrado para este usuário.");
                    }

                    var vinculosDTOS = vinculos.Select(v => new GetVinculoDTO
                    {
                        Id = v.Id,
                        Role = v.Role?.Nome ?? "!!! Role não definida !!!",
                        NomeGranja = v.Granja?.NomePropriedade,
                        NomeAgroindustria = v.Agroindustria?.NomeFantasia
                    }).ToList();

                    return Ok(vinculosDTOS);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar vínculos: " + ex.Message);
            }
        }

        /// <summary>
        /// Endpoint para selecionar um vínculo específico.
        /// Verifica se o vínculo selecionado pertence ao mesmo usuário que o token e, em seguida, gera um novo token de vínculo e atualiza o cookie.
        /// </summary>
        /// <param name="id">ID do vínculo selecionado.</param>
        /// <returns>Retorna a Role do vínculo selecionado.</returns>
        [Authorize(Policy = "AcessoLoginOuTodosUsuarios")]
        [HttpPost("vinculos/{id}")]
        public async Task<IActionResult> SelecionarVinculo(int id)
        {
            try
            {
                // Obtém o valor do NameIdentifier do token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Usuário não autenticado.");
                }

                // Verifica se o token é de acesso (possui a claim "AcessoLogin" com valor "true")
                var isAcessoLogin = User.HasClaim("AcessoLogin", "true");
                int usuarioId;

                if (isAcessoLogin)
                {
                    // Token de acesso: o NameIdentifier é o ID do usuário
                    Console.WriteLine("Token de acesso detectado. NameIdentifier é o ID do usuário.");
                    usuarioId = int.Parse(userIdClaim);
                }
                else
                {
                    // Token de vínculo: o NameIdentifier é o ID do vínculo; buscar para obter o usuário associado
                    Console.WriteLine("Token de vínculo detectado. NameIdentifier é o ID do vínculo.");
                    var vinculoIdClaim = int.Parse(userIdClaim);

                    // Busca o vínculo atual para obter o ID do usuário associado
                    var vinculoAtual = await _context.Vinculos
                        .FirstOrDefaultAsync(v => v.Id == vinculoIdClaim);

                    if (vinculoAtual == null)
                    {
                        return Unauthorized("Vínculo atual não encontrado.");
                    }

                    usuarioId = vinculoAtual.UsuarioId;
                }

                // Busca o vínculo selecionado e verifica se pertence ao mesmo usuário
                var vinculo = await _context.Vinculos
                    .Include(v => v.Role)
                    .Include(v => v.Granja)
                    .Include(v => v.Agroindustria)
                    .FirstOrDefaultAsync(v => v.Id == id && v.UsuarioId == usuarioId);

                if (vinculo == null)
                {
                    return NotFound("Vínculo não encontrado ou não pertence ao mesmo usuário.");
                }

                // Informações para retorno
                var role = vinculo.Role?.Nome ?? "!!! Role não definida !!!";

                // Extrai informações para geração de novo token
                var newVinculoId = vinculo.Id.ToString();
                var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = Request.Headers["User-Agent"].ToString();

                // Gera novo token para o vínculo selecionado
                var token = _tokenService.GenerateTokenVinculo(newVinculoId, userIp, userAgent);

                // Atualiza o cookie HTTP-Only com o novo token
                _tokenService.SetCookieToken(HttpContext, token);

                return Ok(new { Role = role });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao selecionar vínculo: " + ex.Message);
            }
        }

        /// <summary>
        /// Endpoint para processar a recuperação de senha.
        /// Gera uma nova senha aleatória para o usuário com base no email informado,
        /// atualiza a senha no banco e envia a nova senha por email.
        /// </summary>
        /// <param name="model">Objeto contendo o email do usuário.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [HttpPost("recuperar-senha")]
        public async Task<IActionResult> ProcessarRecuperacaoSenha([FromBody] RecuperarSenhaDto model)
        {
            try
            {
                // Busca o usuário pelo email informado
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                // Gera uma nova senha aleatória com tamanho 6
                var novaSenha = _passwordService.GenerateRandomPassword(6);

                // Gera salt e hash para a nova senha
                var salt = _passwordService.GenerateSalt();
                var hash = _passwordService.HashPassword(novaSenha, salt);

                // Atualiza a senha do usuário no banco de dados
                var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == usuario.Id);
                if (senha == null)
                {
                    return BadRequest("Erro ao redefinir a senha.");
                }

                senha.SenhaHash = hash;
                senha.Salt = salt;

                _context.Senhas.Update(senha);
                await _context.SaveChangesAsync();

                // Envia a nova senha por email para o usuário
                await _emailService.EnviarEmailRecuperacaoSenha(usuario.Email, novaSenha);

                return Ok(new { message = "Nova senha enviada para o email informado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao recuperar senha: " + ex.Message);
            }
        }

        /// <summary>
        /// Endpoint para verificar o status de autenticação.
        /// Retorna informações básicas contidas nas claims do token.
        /// Caso o token seja de acesso, retorna status "logado";
        /// caso contrário, retorna status "autenticado" com algumas claims adicionais.
        /// </summary>
        /// <returns>Status de autenticação e informações das claims.</returns>
        [HttpGet("check")]
        public IActionResult CheckStatus()
        {
            try
            {
                // Extrai informações das claims do token
                var claims = new
                {
                    Role = User.FindFirst(ClaimTypes.Role)?.Value,
                    Granja = User.FindFirst("Granja")?.Value,
                    Agroindustria = User.FindFirst("Agroindustria")?.Value,
                    UsuarioNome = User.FindFirst("UsuarioNome")?.Value,
                    UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                };

                if (string.IsNullOrEmpty(claims.UserId))
                {
                    return Unauthorized(new { status = "invalido" });
                }

                // Se o token for de acesso (contém a claim "AcessoLogin")
                if (User.HasClaim("AcessoLogin", "true"))
                {
                    return Ok(new { status = "logado" });
                }

                // Se não, considera como token de vínculo e retorna informações adicionais
                return Ok(new
                {
                    status = "autenticado",
                    claims.Role,
                    claims.Granja,
                    claims.Agroindustria,
                    claims.UsuarioNome
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    status = "erro",
                    message = "Erro ao verificar status de autenticação.",
                    details = ex.Message
                });
            }
        }
    }
}
