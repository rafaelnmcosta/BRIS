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

        public AutenticacaoController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
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
                AgroindustriaId = modelUsuario.AgroindustriaId
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var salt = PasswordService.GenerateSalt();
            var hash = PasswordService.HashPassword(modelUsuario.Senha, salt);

            var senha = new Senha
            {
                UsuarioId = usuario.Id,
                SenhaHash = hash,
                Salt = salt
            };

            _context.Senhas.Add(senha);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto modelLogin)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Senha)
                .FirstOrDefaultAsync(u => u.Email == modelLogin.Email);

            if (usuario == null || !PasswordService.VerifyPassword(modelLogin.Senha, usuario.Senha.Salt, usuario.Senha.SenhaHash))
            {
                return Unauthorized();
            }

            return Ok(new { userId = usuario.Id });
        }

        
        [HttpGet("registros/{id}")]
        public async Task<IActionResult> GetRegistros(int id)
        {
            var usuarioExists = await _context.Usuarios.AnyAsync(u => u.Id == id);
            if (!usuarioExists)
            {
                return NotFound();
            }

            var registros = await _context.GranjasUsuariosTipos
                .Where(gut => gut.UsuarioId == id)
                .Join(_context.TiposUsuario,
                    gut => gut.TipoUsuarioId,
                    tipo => tipo.Id,
                    (gut, tipo) => new { gut, tipo })
                .Join(_context.Granjas,
                    combined => combined.gut.GranjaId,
                    granja => granja.Id,
                    (combined, granja) => new RegistroDTO
                    {
                        Id = combined.gut.Id,
                        NomeTipo = combined.tipo.Tipo,
                        TipoId = combined.gut.TipoUsuarioId,
                        NomeGranja = granja.NomePropriedade,
                        GranjaId = combined.gut.GranjaId
                    })
                .ToListAsync();

            return Ok(new { registros });
        }

        [HttpPost("registros/token/{id}/")]
        public async Task<IActionResult> GenerateToken(int id)
        {
            // Busca a entidade GranjasUsuariosTipos pelo ID
            var granjaUsuarioTipo = await _context.GranjasUsuariosTipos
                .Include(gut => gut.Usuario)
                .Include(gut => gut.Granja)
                .FirstOrDefaultAsync(gut => gut.Id == id);

            if (granjaUsuarioTipo == null)
            {
                return NotFound("GranjaUsuarioTipo não encontrado.");
            }

            var usuarioId = granjaUsuarioTipo.UsuarioId.ToString();
            var tipoUsuarioId = granjaUsuarioTipo.TipoUsuarioId.ToString();
            var granjaId = granjaUsuarioTipo.GranjaId.ToString();
            var agroindustriaId = granjaUsuarioTipo.Usuario.AgroindustriaId.ToString();

            // Gera o token usando o serviço de token
            var token = _tokenService.GenerateToken(usuarioId, tipoUsuarioId, granjaId, agroindustriaId);

            // Retorna o token gerado
            return Ok(new { token });
        }
    }
}