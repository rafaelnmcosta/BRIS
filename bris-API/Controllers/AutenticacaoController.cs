using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
        public async Task<IActionResult> Cadastro([FromBody] CadastroDto modelUsuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == modelUsuario.Email))
                return BadRequest("Já existe um usuário com esse email!");

            var usuario = new Usuario
            {
                Nome = modelUsuario.Nome,
                Email = modelUsuario.Email,
                CPF = modelUsuario.CPF,
                AgroindustriaId = 1
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

            var novoAcesso = new GranjaUsuarioTipo
            {
                UsuarioId = usuario.Id,
                GranjaId = null,
                TipoUsuarioId = 98
            };
            _context.GranjasUsuariosTipos.Add(novoAcesso);

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

            return Ok(new { userId = usuario.Id });
        }

        
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
            var acessos = await _context.GranjasUsuariosTipos
                .Where(gut => gut.UsuarioId == id)
                .Join(_context.TiposUsuario,
                    gut => gut.TipoUsuarioId,
                    tipo => tipo.Id,
                    (gut, tipo) => new { gut, tipo })
                .GroupJoin(_context.Granjas,
                    combined => combined.gut.GranjaId,
                    granja => granja.Id,
                    (combined, granjas) => new { combined.gut, combined.tipo, granja = granjas.FirstOrDefault() })
                .Select(result => new AcessoDTO
                {
                    Id = result.gut.Id,
                    NomeTipo = result.tipo.Tipo,
                    TipoId = result.gut.TipoUsuarioId,
                    NomeGranja = result.granja != null ? result.granja.NomePropriedade : null,
                    GranjaId = result.gut.GranjaId
                })
                .ToListAsync();

            return Ok(new { acessos });
        }


        [HttpPost("acessos/token/{id}/")]
        public async Task<IActionResult> GenerateToken(int id)
        {
            // Busca a entidade GranjasUsuariosTipos pelo ID e inclui TipoUsuario para acessar o campo TIPO
            var granjaUsuarioTipo = await _context.GranjasUsuariosTipos
                .Include(gut => gut.Usuario)
                .Include(gut => gut.Granja)
                .Include(gut => gut.TipoUsuario) // Inclui o TipoUsuario para acessar o campo TIPO
                .FirstOrDefaultAsync(gut => gut.Id == id);

            if (granjaUsuarioTipo == null)
            {
                return NotFound("O usuário não possui nenhum vínculo com nenhuma granja ou tipo definido.");
            }

            var usuarioId = granjaUsuarioTipo.UsuarioId.ToString();
            var tipoUsuarioNome = granjaUsuarioTipo.TipoUsuario.Tipo;
            var granjaId = granjaUsuarioTipo.GranjaId.ToString();
            var agroindustriaId = granjaUsuarioTipo.Usuario.AgroindustriaId.ToString();

            // Gera o token usando o serviço de token com o nome da role
            var token = _tokenService.GenerateToken(usuarioId, tipoUsuarioNome, granjaId, agroindustriaId);

            // Retorna o token gerado
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