using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using bris_API.Data;
using bris_API.Models;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    [Route("api/granjas")]
    [ApiController]
    public class GranjasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GranjasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/granjas
        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Granja>>> GetGranjas()
        {
            var granjas = await _context.Granjas.ToListAsync();

            return Ok(granjas);
        }

        // GET: api/granjas/ativar
        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet("ativar")]
        public async Task<ActionResult<IEnumerable<Granja>>> GetGranjasInativas()
        {
            var granjas = await _context.Granjas
                .Where(g => !g.Ativo)
                .ToListAsync();

            return Ok(granjas);
        }

        // PUT: api/granjas/ativar/{id}
        [Authorize(Policy = "GerenciaTotal")]
        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarGranja(int id)
        {
            var granja = await _context.Granjas
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
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
        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Granja>> GetGranja(int id)
        {
            var granja = await _context.Granjas
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
            }

            return Ok(granja);
        }

        // PUT: api/granjas/{id}/editar
        [Authorize(Policy = "GerenciaTotal")]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> PutGranja(int id, [FromBody] AdminEditaGranjaDto modelGranja)
        {
            var granja = await _context.Granjas
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
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
        [Authorize(Policy = "GerenciaTotal")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> PostGranja([FromBody] AdminEditaGranjaDto modelGranja)
        {
            var novaGranja = new Granja
            {
                NomePropriedade = modelGranja.NomePropriedade,
                Endereco = modelGranja.Endereco,
                CNPJ = modelGranja.CNPJ,
                AgroindustriaId = modelGranja.AgroindustriaId,
                Ativo = modelGranja.Ativo
            };

            _context.Granjas.Add(novaGranja);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja cadastrada com sucesso!" });
        }

        // DELETE: api/granjas/{id}/desativar
        [Authorize(Policy = "GerenciaTotal")]
        [HttpDelete("{id}/desativar")]
        public async Task<IActionResult> DeleteGranja(int id)
        {
            var granja = await _context.Granjas
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
            }

            granja.Ativo = false;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja desativada com sucesso!" });
        }
    }
}
