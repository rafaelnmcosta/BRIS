using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using bris_API.Data;
using bris_API.Models;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    [Route("api/animais")]
    [ApiController]
    public class AnimaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AnimaisController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/animais
        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoAgro)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimais()
        {
            var granjaUsuarioTipoId = int.Parse(User.FindFirst("GranjaUsuarioTipoId")?.Value);

            var granjaId = await _context.GranjasUsuariosTipos
                .Where(gut => gut.Id == granjaUsuarioTipoId)
                .Select(gut => gut.GranjaId)
                .FirstOrDefaultAsync();

            var animais = await _context.Animais
                .Where(a => a.GranjaId == granjaId && a.Ativo)
                .ToListAsync();

            return Ok(animais);
        }

        // GET: api/animais/inativas
        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoAgro)]
        [HttpGet("ativar")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimaisInativos()
        {
            var granjaUsuarioTipoId = int.Parse(User.FindFirst("GranjaUsuarioTipoId")?.Value);

            var granjaId = await _context.GranjasUsuariosTipos
                .Where(gut => gut.Id == granjaUsuarioTipoId)
                .Select(gut => gut.GranjaId)
                .FirstOrDefaultAsync();

            var animais = await _context.Animais
                .Where(a => a.GranjaId == granjaId && !a.Ativo)
                .ToListAsync();

            return Ok(animais);
        }

        // PUT: api/animais/ativar/{id}
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAgro)]
        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarAnimal(int id)
        {
            var granjaUsuarioTipoId = int.Parse(User.FindFirst("GranjaUsuarioTipoId")?.Value);

            var granjaId = await _context.GranjasUsuariosTipos
                .Where(gut => gut.Id == granjaUsuarioTipoId)
                .Select(gut => gut.GranjaId)
                .FirstOrDefaultAsync();

            var animal = await _context.Animais
                .Where(a => a.Id == id && a.GranjaId == granjaId)
                .FirstOrDefaultAsync();

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua granja.");
            }

            if (animal.Ativo)
            {
                return BadRequest("Animal já está ativo.");
            }

            animal.Ativo = true;

            _context.Entry(animal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal ativado com sucesso!" });
        }

        // GET: api/animais/{id}
        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoAgro)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var granjaUsuarioTipoId = int.Parse(User.FindFirst("GranjaUsuarioTipoId")?.Value);

            var granjaId = await _context.GranjasUsuariosTipos
                .Where(gut => gut.Id == granjaUsuarioTipoId)
                .Select(gut => gut.GranjaId)
                .FirstOrDefaultAsync();

            var animal = await _context.Animais
                .Where(a => a.Id == id && a.GranjaId == granjaId)
                .FirstOrDefaultAsync();

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua granja.");
            }

            return Ok(animal);
        }

        // PUT: api/animais/{id}
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAgro)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, [FromBody] AnimalDto modelAnimal)
        {
            var granjaUsuarioTipoId = int.Parse(User.FindFirst("GranjaUsuarioTipoId")?.Value);

            var granjaId = await _context.GranjasUsuariosTipos
                .Where(gut => gut.Id == granjaUsuarioTipoId)
                .Select(gut => gut.GranjaId)
                .FirstOrDefaultAsync();

            var animal = await _context.Animais
                .Where(a => a.Id == id && a.GranjaId == granjaId)
                .FirstOrDefaultAsync();

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua granja.");
            }

            // Atualiza os campos do animal
            animal.Linhagem = modelAnimal.Linhagem;
            animal.Idade = modelAnimal.Idade;
            animal.Peso = modelAnimal.Peso;
            animal.Status = modelAnimal.status;
            animal.UsuarioResponsavelId = modelAnimal.UsuarioResponsavelIdId;
            animal.Ativo = modelAnimal.Ativo;

            _context.Entry(animal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal atualizado com sucesso!" });
        }

        // POST: api/animais
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAgro)]
        [HttpPost]
        public async Task<IActionResult> PostAnimal([FromBody] AnimalDto modelAnimal)
        {
            var granjaUsuarioTipoId = int.Parse(User.FindFirst("GranjaUsuarioTipoId")?.Value);

            var granjaId = await _context.GranjasUsuariosTipos
                .Where(gut => gut.Id == granjaUsuarioTipoId)
                .Select(gut => gut.GranjaId)
                .FirstOrDefaultAsync();

            var novoAnimal = new Animal
            {
                Linhagem = modelAnimal.Linhagem,
                Idade = modelAnimal.Idade,
                Peso = modelAnimal.Peso,
                Status = modelAnimal.status,
                UsuarioResponsavelId = modelAnimal.UsuarioResponsavelIdId,
                Ativo = modelAnimal.Ativo,
                GranjaId = granjaId
            };

            _context.Animais.Add(novoAnimal);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal cadastrado com sucesso!" });
        }

        // DELETE: api/animais/{id}
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAgro)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var granjaUsuarioTipoId = int.Parse(User.FindFirst("GranjaUsuarioTipoId")?.Value);

            var granjaId = await _context.GranjasUsuariosTipos
                .Where(gut => gut.Id == granjaUsuarioTipoId)
                .Select(gut => gut.GranjaId)
                .FirstOrDefaultAsync();

            var animal = await _context.Animais
                .Where(a => a.Id == id && a.GranjaId == granjaId)
                .FirstOrDefaultAsync();

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua granja.");
            }

            animal.Ativo = false;

            _context.Entry(animal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal desativado com sucesso!" });
        }
    }
}
