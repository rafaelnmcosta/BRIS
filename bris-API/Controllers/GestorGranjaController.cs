using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using bris_API.Data;
using bris_API.Models;
using bris_API.Services;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    [Route("api/gg")]
    [ApiController]
    public class GestorGranjaController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GestorGranjaController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
        }

        // GET: api/gg/usuarios
        [Authorize(Policy = "VisualizaGranja")]
        [HttpGet("usuarios")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;

            if (string.IsNullOrEmpty(granjaId))
            {
                return Unauthorized("GranjaId não encontrado no token.");
            }

            var usuarios = await _context.GranjasUsuariosTipos
                .Where(gut => gut.GranjaId == int.Parse(granjaId))
                .Select(gut => gut.Usuario)
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/gg/usuarios/{id}
        [Authorize(Policy = "VisualizaGranja")]
        [HttpGet("usuarios/{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;

            if (string.IsNullOrEmpty(granjaId))
            {
                return Unauthorized("GranjaId não encontrado no token.");
            }

            var usuario = await _context.Usuarios
                .Where(u => u.Id == id && _context.GranjasUsuariosTipos
                    .Any(gut => gut.UsuarioId == u.Id && gut.GranjaId == int.Parse(granjaId)))
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado ou não pertence à sua granja.");
            }

            return Ok(usuario);
        }

        // PUT: api/gg/usuarios/{id}/editar
        [Authorize(Policy = "GerenciaGranja")]
        [HttpPut("usuarios/{id}/editar")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] EditarGestorGranjaDto modelUsuario)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;

            if (string.IsNullOrEmpty(granjaId))
            {
                return Unauthorized("GranjaId não encontrado no token.");
            }

            var usuario = await _context.Usuarios
                .Where(u => u.Id == id && _context.GranjasUsuariosTipos
                    .Any(gut => gut.UsuarioId == u.Id && gut.GranjaId == int.Parse(granjaId)))
                .FirstOrDefaultAsync();

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado ou não pertence à sua granja.");
            }

            // Atualiza as informações do usuário
            usuario.Nome = modelUsuario.Nome;
            usuario.Email = modelUsuario.Email;
            usuario.CPF = modelUsuario.CPF;

            // Atualiza a senha
            var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == id);
            if (senha != null)
            {
                var salt = PasswordService.GenerateSalt();
                senha.SenhaHash = PasswordService.HashPassword(modelUsuario.Senha, salt);
                senha.Salt = salt;
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário atualizado com sucesso!" });
        }

        // POST: api/gg/cadastrar
        [Authorize(Policy = "GerenciaGranja")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CadastroGestorGranjaDto modelUsuario)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            var agroindustriaId = User.FindFirst("AgroindustriaId")?.Value;

            if (string.IsNullOrEmpty(granjaId) || string.IsNullOrEmpty(agroindustriaId))
            {
                return Unauthorized("GranjaId ou AgroindustriaId não encontrados no token.");
            }

            var novoUsuario = new Usuario
            {
                Nome = modelUsuario.Nome,
                Email = modelUsuario.Email,
                CPF = modelUsuario.CPF,
                AgroindustriaId = int.Parse(agroindustriaId)
            };

            _context.Usuarios.Add(novoUsuario);
            await _context.SaveChangesAsync();

            // Lógica para armazenar a senha
            var salt = PasswordService.GenerateSalt();
            var hash = PasswordService.HashPassword(modelUsuario.Senha, salt);

            var senha = new Senha
            {
                UsuarioId = novoUsuario.Id,
                SenhaHash = hash,
                Salt = salt
            };

            _context.Senhas.Add(senha);
            await _context.SaveChangesAsync();

            // Cria registro em GranjasUsuariosTipos
            var granjaUsuarioTipo = new GranjaUsuarioTipo
            {
                UsuarioId = novoUsuario.Id,
                GranjaId = int.Parse(granjaId),
                TipoUsuarioId = 4 // O gestor da granja só cadastra novos técnicos
            };

            _context.GranjasUsuariosTipos.Add(granjaUsuarioTipo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        // GET: api/gg/ativar
        [Authorize(Policy = "VisualizaGranja")]
        [HttpGet("ativar")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosParaAtivar()
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;

            if (string.IsNullOrEmpty(granjaId))
            {
                return Unauthorized("GranjaId não encontrado no token.");
            }

            var usuarios = await _context.GranjasUsuariosTipos
                .Where(gut => gut.GranjaId == int.Parse(granjaId) && (gut.TipoUsuarioId == 98 || gut.TipoUsuarioId == 99))
                .Select(gut => gut.Usuario)
                .ToListAsync();

            return Ok(usuarios);
        }

        // POST: api/gg/ativar/{id}
        [Authorize(Policy = "GerenciaGranja")]
        [HttpPost("ativar/{id}")]
        public async Task<IActionResult> AtivarUsuario(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;

            if (string.IsNullOrEmpty(granjaId))
            {
                return Unauthorized("GranjaId não encontrado no token.");
            }

            var granjaUsuarioTipo = await _context.GranjasUsuariosTipos
                .Where(gut => gut.UsuarioId == id && gut.GranjaId == int.Parse(granjaId) && (gut.TipoUsuarioId == 98 || gut.TipoUsuarioId == 99))
                .FirstOrDefaultAsync();

            if (granjaUsuarioTipo == null)
            {
                return NotFound("Registro de ativação não encontrado ou não pertence à sua granja.");
            }

            granjaUsuarioTipo.TipoUsuarioId = 4; // Gestor de Granja apenas ativa usuários para o tipo 4

            _context.Entry(granjaUsuarioTipo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário ativado com sucesso!" });
        }
    }
}
