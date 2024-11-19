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

        [Authorize(Policy = "TodosUsuarios")]
        [HttpGet("trocar-vinculo")]
        public async Task<IActionResult> TrocarVinculo()
        {
            try
            {
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Pega o Id do usuário através do vínculo dele
                var vinculoAtual = await _context.Vinculos
                    .FirstOrDefaultAsync(v => v.Id == vinculoId);
                var usuarioId = vinculoAtual.UsuarioId;

                // Busca os outros vínculos do usuário no banco
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
                    VinculoId = v.Id,
                    Role = v.Role?.Nome ?? "!!! Role não definida !!!",
                    NomeGranja = v.Granja?.NomePropriedade,
                    NomeAgroindustria = v.Agroindustria?.NomeFantasia
                }).ToList();

                // Remove o token anterior
                HttpContext.Response.Cookies.Delete("auth_token");

                // Obtém informações da requisição para gerar o token para a seleção de vínculo
                var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = Request.Headers["User-Agent"].ToString();

                // Gera o novo token e o adiciona ao cookie
                var token = _tokenService.GenerateTokenLogin(usuarioId.ToString(), userIp, userAgent);
                _tokenService.SetCookieToken(HttpContext, token);

                // retorna a lista de vínculos do usuário para que ele possa selecionar um novo
                return Ok(vinculosDTOS);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao trocar o vínculo ativo: " + ex.Message);
            }
        }

        [Authorize(Policy = "AcessoLoginPolicy")]
        [HttpGet("vinculos")]
        public async Task<IActionResult> GetVinculos()
        {
            try
            {
                var usuarioClaimId = User.FindFirst(ClaimTypes.NameIdentifier);
                Console.WriteLine("usuarioClaimId = " + usuarioClaimId.Value);
                if (usuarioClaimId == null)
                {
                    return Unauthorized("Token inválido. (Id na claim do token é null)");
                }

                var usuarioId = int.Parse(usuarioClaimId.Value); // id do usuario em formato int

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
                    VinculoId = v.Id,
                    Role = v.Role?.Nome ?? "!!! Role não definida !!!",
                    NomeGranja = v.Granja?.NomePropriedade,
                    NomeAgroindustria = v.Agroindustria?.NomeFantasia
                }).ToList();

                return Ok(vinculosDTOS);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar vínculos: " + ex.Message);
            }
        }

        [Authorize(Policy = "AcessoLoginPolicy")]
        [HttpPost("vinculos/{id}")]
        public async Task<IActionResult> SelecionarVinculo(int id)
        {
            try
            {
                // Obtem o ID do usuário a partir do token atual
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (userId == null)
                {
                    return Unauthorized("Usuário não autenticado.");
                }

                // Busca o vínculo pelo ID e garante que o usuário tenha permissão
                var vinculo = await _context.Vinculos
                    .Include(v => v.Role)
                    .Include(v => v.Granja)
                    .Include(v => v.Agroindustria)
                    .FirstOrDefaultAsync(v => v.Id == id && v.UsuarioId.ToString() == userId);

                // Verifica se o vínculo existe
                if (vinculo == null)
                {
                    return NotFound("Vínculo não encontrado ou não pertence ao usuário autenticado.");
                }

                // Extrai informações necessárias para gerar o token
                var role = vinculo.Role?.Nome ?? string.Empty;
                var vinculoId = vinculo.Id.ToString();
                var granjaId = vinculo.Granja?.Id.ToString() ?? string.Empty;
                var agroindustriaId = vinculo.Agroindustria?.Id.ToString() ?? string.Empty;

                // Obtem informações do IP e User Agent
                var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
                var userAgent = Request.Headers["User-Agent"].ToString();

                // Gera o novo token
                var token = _tokenService.GenerateTokenVinculo(vinculoId, userIp, userAgent);

                // Configura o cookie HTTP-Only para o token gerado
                _tokenService.SetCookieToken(HttpContext, token);

                // Retorna a Role escolhida pelo usuário
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
        //[Authorize] // Garante que o usuário deve estar autenticado para acessar este endpoint
        public IActionResult CheckLogin()
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new { message = "Usuário não autenticado." });
                }
                
                return Ok(new { message = "Usuário autenticado." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erro ao verificar autenticação.", details = ex.Message });
            }
        }

    }
}