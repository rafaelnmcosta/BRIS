using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using bris_API.Data;
using bris_API.Models;
using bris_API.DTOs;
using bris_API.Services;

namespace bris_API.Controllers
{
    [Route("api/ga")]
    [ApiController]
    public class GestorAgroController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GestorAgroController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/ga/animais
        [Authorize(Policy = "VisualizaAgro")]
        [HttpGet("animais")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimais()
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var animais = await _context.Animais
                .Include(a => a.Granja) // Inclui a granja para verificar a agroindústria
                .Where(a => a.Granja.AgroindustriaId == agroindustriaId)
                .ToListAsync();

            return Ok(animais);
        }

        // GET: api/ga/animais/{id}
        [Authorize(Policy = "VisualizaAgro")]
        [HttpGet("animais/{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var animal = await _context.Animais
                .Include(a => a.Granja) // Inclui a granja para verificar a agroindústria
                .FirstOrDefaultAsync(a => a.Id == id && a.Granja.AgroindustriaId == agroindustriaId);

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua agroindústria.");
            }

            return Ok(animal);
        }

        // GET: api/ga/usuarios
        [Authorize(Policy = "VisualizaAgro")]
        [HttpGet("usuarios")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var usuarios = await _context.Usuarios
                .Where(u => u.AgroindustriaId == agroindustriaId)
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/ga/usuarios/{id}
        [Authorize(Policy = "VisualizaAgro")]
        [HttpGet("usuarios/{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var usuario = await _context.Usuarios
                .Include(u => u.GranjasUsuariosTipos)
                .FirstOrDefaultAsync(u => u.Id == id && u.AgroindustriaId == agroindustriaId);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado ou não pertence à sua agroindústria.");
            }

            return Ok(usuario);
        }

        // PUT: api/ga/usuarios/{id}/editar
        [Authorize(Policy = "GerenciaAgro")]
        [HttpPut("usuarios/{id}/editar")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] GestorAgroEditaUsuarioDto modelUsuario)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var usuario = await _context.Usuarios
                .Include(u => u.GranjasUsuariosTipos)
                .FirstOrDefaultAsync(u => u.Id == id && u.AgroindustriaId == agroindustriaId);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado ou não pertence à sua agroindústria.");
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

            // Atualiza ou adiciona os registros em GranjasUsuariosTipos
            foreach (var nivelAcesso in modelUsuario.NiveisAcesso)
            {
                var acesso = await _context.GranjasUsuariosTipos
                    .FirstOrDefaultAsync(gut => gut.Id == nivelAcesso.Id && gut.UsuarioId == id);

                if (acesso != null)
                {
                    acesso.TipoUsuarioId = nivelAcesso.TipoUsuarioId;
                    acesso.GranjaId = nivelAcesso.GranjaId ?? acesso.GranjaId;
                }
                else
                {
                    // Lógica para adicionar um novo acesso, se necessário
                    var novoAcesso = new Vinculos
                    {
                        UsuarioId = id,
                        TipoUsuarioId = nivelAcesso.TipoUsuarioId,
                        GranjaId = nivelAcesso.GranjaId ?? default(int)
                    };
                    _context.GranjasUsuariosTipos.Add(novoAcesso);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário e níveis de acesso atualizados com sucesso!" });
        }

        // POST: api/ga/usuarios/cadastrar
        [Authorize(Policy = "GerenciaAgro")]
        [HttpPost("usuarios/cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] GestorAgroCadastraUsuarioDto modelUsuario)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var novoUsuario = new Usuario
            {
                Nome = modelUsuario.Nome,
                Email = modelUsuario.Email,
                CPF = modelUsuario.CPF,
                AgroindustriaId = agroindustriaId,
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

            // Cria acesso em GranjasUsuariosTipos
            var granjaUsuarioTipo = new Vinculos
            {
                UsuarioId = novoUsuario.Id,
                GranjaId = modelUsuario.GranjaId,
                TipoUsuarioId = modelUsuario.TipoUsuarioId
            };

            _context.GranjasUsuariosTipos.Add(granjaUsuarioTipo);
            await _context.SaveChangesAsync();


            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        // GET: api/ga/usuarios/ativar
        [Authorize(Policy = "VisualizaAgro")]
        [HttpGet("usuarios/ativar")]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuariosParaAtivar()
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var usuarios = await _context.Usuarios
                .Where(u => u.AgroindustriaId == agroindustriaId)
                .Where(u => u.GranjasUsuariosTipos.Any(gut => gut.TipoUsuarioId == 98 || gut.TipoUsuarioId == 99))
                .ToListAsync();

            return Ok(usuarios);
        }


        // POST: api/ga/usuarios/ativar/{id}
        [Authorize(Policy = "GerenciaAgro")]
        [HttpPut("usuarios/ativar/{id}")]
        public async Task<IActionResult> AtivarUsuario(int id, [FromBody] AtivarDto ativarDTO)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var granjaUsuarioTipo = await _context.GranjasUsuariosTipos
                .Include(gut => gut.Usuario)
                .FirstOrDefaultAsync(gut => gut.UsuarioId == id && (gut.TipoUsuarioId == 98 || gut.TipoUsuarioId == 99) && gut.Usuario.AgroindustriaId == agroindustriaId);

            if (granjaUsuarioTipo == null)
            {
                return NotFound("Registro de ativação não encontrado ou não pertence à sua agroindústria.");
            }

            granjaUsuarioTipo.TipoUsuarioId = ativarDTO.TipoUsuario;
            granjaUsuarioTipo.GranjaId = ativarDTO.GranjaId;

            _context.Entry(granjaUsuarioTipo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário ativado com sucesso!" });
        }

        // GET: api/granjas
        [Authorize(Policy = "VisualizaAgro")]
        [HttpGet("granjas")]
        public async Task<ActionResult<IEnumerable<Granja>>> GetGranjas()
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var granjas = await _context.Granjas
                .Where(g => g.AgroindustriaId == agroindustriaId && g.Ativo)
                .ToListAsync();

            return Ok(granjas);
        }

        // GET: api/granjas/ativar
        [Authorize(Policy = "VisualizaAgro")]
        [HttpGet("granjas/ativar")]
        public async Task<ActionResult<IEnumerable<Granja>>> GetGranjasInativas()
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var granjas = await _context.Granjas
                .Where(g => g.AgroindustriaId == agroindustriaId && !g.Ativo)
                .ToListAsync();

            return Ok(granjas);
        }

        // PUT: api/granjas/ativar/{id}
        [Authorize(Policy = "GerenciaAgro")]
        [HttpPut("granjas/ativar/{id}")]
        public async Task<IActionResult> AtivarGranja(int id)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var granja = await _context.Granjas
                .Where(g => g.Id == id && g.AgroindustriaId == agroindustriaId)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada ou não pertence à sua agroindústria.");
            }

            if (granja.Ativo)
            {
                return BadRequest("Granja já está ativa.");
            }

            granja.Ativo = true;

            _context.Entry(granja).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja ativada com sucesso!" });
        }

        // GET: api/granjas/{id}
        [Authorize(Policy = "VisualizaAgro")]
        [HttpGet("granjas/{id}")]
        public async Task<ActionResult<Granja>> GetGranja(int id)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var granja = await _context.Granjas
                .Where(g => g.Id == id && g.AgroindustriaId == agroindustriaId)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada ou não pertence à sua agroindústria.");
            }

            return Ok(granja);
        }

        // PUT: api/granjas/{id}/editar
        [Authorize(Policy = "GerenciaAgro")]
        [HttpPut("granjas/{id}/editar")]
        public async Task<IActionResult> PutGranja(int id, [FromBody] GestorAgroEditaGranjaDto modelGranja)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var granja = await _context.Granjas
                .Where(g => g.Id == id && g.AgroindustriaId == agroindustriaId)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada ou não pertence à sua agroindústria.");
            }

            // Atualiza os campos da granja
            granja.NomePropriedade = modelGranja.NomePropriedade;
            granja.Endereco = modelGranja.Endereco;
            granja.CNPJ = modelGranja.CNPJ;
            granja.Ativo = modelGranja.Ativo;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja atualizada com sucesso!" });
        }

        // POST: api/granjas/cadastrar
        [Authorize(Policy = "GerenciaAgro")]
        [HttpPost("granjas/cadastrar")]
        public async Task<IActionResult> PostGranja([FromBody] GestorAgroEditaGranjaDto modelGranja)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var novaGranja = new Granja
            {
                NomePropriedade = modelGranja.NomePropriedade,
                Endereco = modelGranja.Endereco,
                CNPJ = modelGranja.CNPJ,
                AgroindustriaId = agroindustriaId,
                Ativo = modelGranja.Ativo
            };

            _context.Granjas.Add(novaGranja);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja cadastrada com sucesso!" });
        }

        // DELETE: api/granjas/{id}/desativar
        [Authorize(Policy = "GerenciaAgro")]
        [HttpDelete("granjas/{id}/desativar")]
        public async Task<IActionResult> DeleteGranja(int id)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var granja = await _context.Granjas
                .Where(g => g.Id == id && g.AgroindustriaId == agroindustriaId)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada ou não pertence à sua agroindústria.");
            }

            granja.Ativo = false;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja desativada com sucesso!" });
        }
    }
}
