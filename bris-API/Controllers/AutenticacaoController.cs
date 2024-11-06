using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using bris_API.Data;
using bris_API.Models;
using bris_API.Services;
using bris_API.DTOs;
using Microsoft.AspNetCore.Authorization;

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
        public async Task<IActionResult> Cadastro([FromBody] CadastroDto modelUsuario)
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
                TipoUsuarioId = 98
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

            return Ok(new { usuario.Id });
        }

        [Authorize(Policy = "TodosUsuarios")]
        [HttpGet("acessos/{id}")]
        public async Task<IActionResult> GetAcessos(int id)
        {
            // Verifica se o usuário existe
            var usuarioExists = await _context.Usuarios.AnyAsync(u => u.Id == id);
            if (!usuarioExists)
            {
                return NotFound("Usuário não encontrado");
            }

            // Busca acessos com junção à esquerda na tabela Granjas
            var acessos = await _context.Vinculos
                .Where(v => v.UsuarioId == id)
                .Join(_context.TiposUsuario,
                    v => v.TipoUsuarioId,
                    tipo => tipo.Id,
                    (v, tipo) => new { v, tipo })
                .GroupJoin(_context.Granjas,
                    combined => combined.v.GranjaId,
                    granja => granja.Id,
                    (combined, granjas) => new { combined.v, combined.tipo, granja = granjas.FirstOrDefault() })
                .Select(result => new AcessoDTO
                {
                    Id = result.v.Id,
                    NomeTipo = result.tipo.Tipo,
                    TipoId = result.v.TipoUsuarioId,
                    NomeGranja = result.granja != null ? result.granja.NomePropriedade : null,
                    GranjaId = result.v.GranjaId
                })
                .ToListAsync();

            return Ok(new { acessos });
        }

        [Authorize(Policy = "TodosUsuarios")]
        [HttpPost("acessos/token/{id}/")]
        public async Task<IActionResult> GenerateTokenVinculo(int id)
        {
            // Busca a entidade Vinculos pelo ID e inclui TipoUsuario para acessar o campo TIPO
            var vinculo = await _context.Vinculos
                .Include(v => v.Usuario)
                .Include(v => v.Granja)
                .Include(v => v.TipoUsuario)
                .Include(v => v.Agroindustria)
                .FirstOrDefaultAsync(v => v.Id == id);

            if (vinculo == null)
            {
                return NotFound("O usuário não possui nenhum vínculo registrado no sistema.");
            }

            var usuarioId = vinculo.UsuarioId.ToString();
            var role = vinculo.TipoUsuario.Tipo.ToString();
            var granjaId = vinculo.GranjaId.ToString();
            var agroindustriaId = vinculo.AgroindustriaId.ToString();
            var ipUsuario = HttpContext.Connection.RemoteIpAddress?.ToString();
            var agentUsuario = HttpContext.Request.Headers["User-Agent"].ToString(); //navegador

            var token = _tokenService.GenerateTokenVinculo(usuarioId, role, granjaId, agroindustriaId, ipUsuario, agentUsuario);

            // Retorna o token completo gerado
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