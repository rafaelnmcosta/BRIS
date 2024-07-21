using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using bris_API.Data;
using bris_API.Models;
using bris_API.Services;

namespace bris_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly ITokenService _tokenService;

        public UsuariosController(AppDbContext context, ITokenService tokenService)
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
                TipoUsuarioId = 0 // TipoUsuarioId 0 para indicar que precisa ser ativado
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
                .Include(u => u.TipoUsuario)
                .FirstOrDefaultAsync(u => u.Email == modelLogin.Email);

            
            if (usuario == null || !PasswordService.VerifyPassword(modelLogin.Senha, usuario.Senha.Salt, usuario.Senha.SenhaHash))
            {
                return Unauthorized();
            }


            var token = _tokenService.GenerateToken(usuario.Id.ToString(), usuario.Email, usuario.TipoUsuario.Nome);
            return Ok(new { token });
        }

        [Authorize(Roles = "Admin,Gerente")]
        [HttpGet("usuarios")]
        public async Task<IActionResult> GetUsuarios()
        {
            return Ok(await _context.Usuarios.ToListAsync());
        }

        [Authorize(Roles = "Admin,Gerente")]
        [HttpPost("usuarios/cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CadastroCompletoDto modelUsuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == modelUsuario.Email))
                return BadRequest("Já existe um usuário com esse email!");

            var usuario = new Usuario
            {
                Nome = modelUsuario.Nome,
                Email = modelUsuario.Email,
                TipoUsuarioId = modelUsuario.TipoUsuario
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

        [Authorize(Roles = "Admin,Gerente")]
        [HttpGet("usuarios/{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [Authorize(Roles = "Admin,Gerente")]
        [HttpGet("usuarios/ativar")]
        public async Task<IActionResult> GetUsuariosNaoAtivados()
        {
            // Buscar todos os usuários com TipoUsuarioId igual a 0
            var usuariosNaoAtivados = await _context.Usuarios
                .Where(u => u.TipoUsuarioId == 0)
                .ToListAsync();

            return Ok(usuariosNaoAtivados);
        }


        [Authorize(Roles = "Admin,Gerente")]
        [HttpPost("usuarios/ativar/{id}")]
        public async Task<IActionResult> AtivarUsuario(int id, [FromBody] AtivarDto ativar)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            Console.WriteLine(ativar.TipoUsuario);
            Console.WriteLine(usuario.Nome);

            usuario.TipoUsuarioId = ativar.TipoUsuario;
            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        [Authorize(Roles = "Admin,Gerente")]
        [HttpPut("usuarios/{id}/editar")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] Usuario usuarioEditado)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Nome = usuarioEditado.Nome;
            usuario.Email = usuarioEditado.Email;
            usuario.TipoUsuarioId = usuarioEditado.TipoUsuarioId;

            await _context.SaveChangesAsync();

            return Ok(usuario);
        }

        [HttpGet("perfil/{id}")]
        public async Task<IActionResult> GetPerfil(int id)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (id != usuarioId)
            {
                return Forbid();
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return Ok(usuario);
        }

        [HttpPut("perfil/{id}/editar")]
        public async Task<IActionResult> EditarPerfil(int id, [FromBody] Usuario usuarioEditado)
        {
            var usuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            if (id != usuarioId)
            {
                return Forbid();
            }

            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            usuario.Nome = usuarioEditado.Nome;
            usuario.Email = usuarioEditado.Email;

            await _context.SaveChangesAsync();

            return Ok(usuario);
        }
    }

    public class CadastroDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
    }

    public class CadastroCompletoDto
    {
        public string Nome { get; set; }
        public string Email { get; set; }
        public int TipoUsuario { get; set; }
        public string Senha { get; set; }
    }

    public class LoginDto
    {
        public string Email { get; set; }
        public string Senha { get; set; }
    }
    public class AtivarDto
    {
        public int TipoUsuario { get; set; }
    }
}