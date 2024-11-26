using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using bris_API.Data;
using bris_API.Models;
using bris_API.Services;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AutenticacaoController : ControllerBase, IAutenticacaoController
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;
        private readonly IPasswordService _passwordService;
        private readonly IEmailService _emailService;

        public AutenticacaoController(AppDbContext context, ITokenService tokenService, IPasswordService passwordService, IEmailService emailService)
        {
            _context = context;
            _tokenService = tokenService;
            _passwordService = passwordService;
            _emailService = emailService;
        }


        [HttpPost("cadastro")]
        public async Task<IActionResult> Cadastro([FromBody] AutoCadastroDTO modelUsuario)
        {
            try
            {
                if (await _context.Usuarios.AnyAsync(u => u.Email == modelUsuario.Email))
                    return BadRequest("Já existe um usuário com esse email!");

                var usuario = new Usuario
                {
                    Nome = modelUsuario.Nome,
                    Email = modelUsuario.Email,
                    CPF = modelUsuario.CPF,
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var salt = _passwordService.GenerateSalt();
                var hash = _passwordService.HashPassword(modelUsuario.Senha, salt);

                var senha = new Senha
                {
                    UsuarioId = usuario.Id,
                    SenhaHash = hash,
                    Salt = salt
                };
                _context.Senhas.Add(senha);

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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto modelLogin)
        {
            try
            {
                var usuario = await _context.Usuarios
                    .Include(u => u.Senha)
                    .FirstOrDefaultAsync(u => u.Email == modelLogin.Email);

                if (usuario == null || !_passwordService.VerifyPassword(modelLogin.Senha, usuario.Senha.Salt, usuario.Senha.SenhaHash))
                {
                    return Unauthorized();
                }

                // Obtém informações da requisição
                var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = Request.Headers["User-Agent"].ToString();

                // Gera o token JWT
                var token = _tokenService.GenerateTokenLogin(usuario.Id.ToString(), userIp, userAgent);

                // Configura o cookie HTTP-Only para o token gerado
                _tokenService.SetCookieToken(HttpContext, token);

                // Retorna a Role escolhida pelo usuário
                return Ok(new { message = "Login efetuado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao efetuar login: " + ex.Message);
            }
        }

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

        [Authorize(Policy = "AcessoLoginOuTodosUsuarios")]
        [HttpGet("vinculos")]
        public async Task<IActionResult> GetVinculos()
        {
            try
            {
                Console.WriteLine("Entrou na getVinculos");
                // Obtém o ID do usuário ou vínculo do token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Token inválido. (Id na claim do token é null)");
                }

                // Verifica se é um token de acesso ou de vínculo
                var isAcessoLogin = User.HasClaim("AcessoLogin", "true");

                if (isAcessoLogin)
                {
                    // Token de acesso: trabalhar com usuário
                    Console.WriteLine("Token de acesso detectado.");
                    var usuarioId = int.Parse(userIdClaim);

                    // Busca os vínculos associados ao usuário
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
                    // Token de vínculo: trabalhar com vínculo específico
                    Console.WriteLine("Token de vínculo detectado.");
                    var vinculoId = int.Parse(userIdClaim);

                    // Busca o vínculo atual
                    var vinculoAtual = await _context.Vinculos
                        .FirstOrDefaultAsync(v => v.Id == vinculoId);

                    if (vinculoAtual == null)
                    {
                        return NotFound("Vínculo atual não encontrado.");
                    }

                    // Busca os outros vínculos do mesmo usuário
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

        [Authorize(Policy = "AcessoLoginOuTodosUsuarios")]
        [HttpPost("vinculos/{id}")]
        public async Task<IActionResult> SelecionarVinculo(int id)
        {
            try
            {
                // Obtem o valor de NameIdentifier do token
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized("Usuário não autenticado.");
                }

                // Verifica o tipo de token pela presença da claim "acessoLogin"
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
                    // Token de vínculo: o NameIdentifier é o ID do vínculo
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

                // Busca o vínculo selecionado pelo ID e verifica se pertence ao mesmo usuário
                var vinculo = await _context.Vinculos
                    .Include(v => v.Role)
                    .Include(v => v.Granja)
                    .Include(v => v.Agroindustria)
                    .FirstOrDefaultAsync(v => v.Id == id && v.UsuarioId == usuarioId);

                // Verifica se o vínculo foi encontrado
                if (vinculo == null)
                {
                    return NotFound("Vínculo não encontrado ou não pertence ao mesmo usuário.");
                }

                // Informações para o retorno
                var role = vinculo.Role?.Nome ?? "!!! Role não definida !!!";

                // Extrai informações necessárias para gerar o token
                var vinculoId = vinculo.Id.ToString();
                var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = Request.Headers["User-Agent"].ToString();

                // Gera o token
                var token = _tokenService.GenerateTokenVinculo(vinculoId, userIp, userAgent);

                // Configura o cookie HTTP-Only com o novo token
                _tokenService.SetCookieToken(HttpContext, token);

                // Retorna a Role escolhida
                return Ok(new { Role = role });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao selecionar vínculo: " + ex.Message);
            }
        }


        // Rota POST para processar o email e redefinir a senha
        [HttpPost("recuperar-senha")]
        public async Task<IActionResult> ProcessarRecuperacaoSenha([FromBody] RecuperarSenhaDto model)
        {
            try
            {
                var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == model.Email);

                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado.");
                }

                // Gerar nova senha aleatória
                var novaSenha = _passwordService.GenerateRandomPassword(6);

                // Criar hash e salt da nova senha
                var salt = _passwordService.GenerateSalt();
                var hash = _passwordService.HashPassword(novaSenha, salt);

                // Atualizar senha no banco de dados
                var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == usuario.Id);
                if (senha == null)
                {
                    return BadRequest("Erro ao redefinir a senha.");
                }

                senha.SenhaHash = hash;
                senha.Salt = salt;

                _context.Senhas.Update(senha);
                await _context.SaveChangesAsync();

                // Enviar nova senha por email
                await _emailService.EnviarEmailRecuperacaoSenha(usuario.Email, novaSenha);

                return Ok(new { message = "Nova senha enviada para o email informado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao recuperar senha: " + ex.Message);
            }
        }

        [HttpGet("check")]
        public IActionResult CheckStatus()
        {
            try
            {
                var roleClaim = User.FindFirst(ClaimTypes.Role)?.Value;
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userIdClaim))
                {
                    return Unauthorized(new { status = "invalido" });
                }

                // Verificando se o token contém a claim "acessoLogin"
                var acessoLoginClaim = User.FindFirst("acessoLogin");
                if (acessoLoginClaim != null)
                {
                    return Ok(new { status = "logado" });
                }


                // Caso contrário, o token é considerado um token de vínculo
                return Ok(new
                {
                    status = "autenticado",
                    role = roleClaim
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