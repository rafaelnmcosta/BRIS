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
    public class AutenticacaoController : ControllerBase
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
        public async Task<IActionResult> Cadastro([FromBody] AutoCadastroDto modelUsuario)
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto modelLogin)
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

            // Retorna o token ao frontend
            return Ok(new { token });
        }


        [Authorize(Policy = "AcessoLoginPolicy")]
        [HttpGet("vinculos")]
        public async Task<IActionResult> GetVinculos()
        {
            var userIdClaim = User.FindFirst(JwtRegisteredClaimNames.Sub);
            if (userIdClaim == null)
            {
                return Unauthorized("Token inválido.");
            }

            var usuarioId = int.Parse(userIdClaim.Value); // id do usuario em formato int

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
                UsuarioId = v.UsuarioId,
                VinculoId = v.Id,
                Role = v.Role?.Nome ?? "Role não definida",
                GranjaId = v.GranjaId,
                NomeGranja = v.Granja?.NomePropriedade,
                AgroindustriaId = v.AgroindustriaId,
                NomeAgroindustria = v.Agroindustria?.NomeFantasia
            }).ToList();

            return Ok(vinculosDTOS);
        }
        
        [Authorize(Policy = "AcessoLoginPolicy")]
        [HttpPost("vinculos/{id}")]
        public async Task<IActionResult> SelecionarVinculo(int id)
        {
            // Obtendo o ID do usuário a partir do token atual
            var userId = User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (userId == null)
            {
                return Unauthorized("Usuário não autenticado.");
            }

            // Buscando o vínculo pelo ID e garantindo que o usuário tenha permissão
            var vinculo = await _context.Vinculos
                .Include(v => v.Role)
                .Include(v => v.Granja)
                .Include(v => v.Agroindustria)
                .FirstOrDefaultAsync(v => v.Id == id && v.UsuarioId.ToString() == userId);

            // Verificando se o vínculo existe
            if (vinculo == null)
            {
                return NotFound("Vínculo não encontrado ou não pertence ao usuário autenticado.");
            }

            // Extraindo informações necessárias para gerar o token
            var role = vinculo.Role?.Nome ?? string.Empty;
            var vinculoId = vinculo.Id.ToString();
            var granjaId = vinculo.Granja?.Id.ToString() ?? string.Empty;
            var agroindustriaId = vinculo.Agroindustria?.Id.ToString() ?? string.Empty;

            // Obtendo informações do IP e User Agent
            var userIp = HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = Request.Headers["User-Agent"].ToString();

            // Gerando o novo token
            var token = _tokenService.GenerateTokenVinculo(userId, vinculoId, role, granjaId, agroindustriaId, userIp, userAgent);

            // Retornando o token gerado
            return Ok(new { token });
        }

        // Rota POST para processar o email e redefinir a senha
        [HttpPost("recuperar-senha")]
        public async Task<IActionResult> ProcessarRecuperacaoSenha([FromBody] RecuperarSenhaDto model)
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

    }
}