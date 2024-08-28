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
        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoAgro)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Granja>>> GetGranjas()
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var granjas = await _context.Granjas
                .Where(g => g.AgroindustriaId == agroindustriaId && g.Ativo)
                .ToListAsync();

            return Ok(granjas);
        }

        // GET: api/granjas/ativar
        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoAgro)]
        [HttpGet("ativar")]
        public async Task<ActionResult<IEnumerable<Granja>>> GetGranjasInativas()
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            var granjas = await _context.Granjas
                .Where(g => g.AgroindustriaId == agroindustriaId && !g.Ativo)
                .ToListAsync();

            return Ok(granjas);
        }

        // PUT: api/granjas/ativar/{id}
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAgro)]
        [HttpPut("ativar/{id}")]
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
        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoAgro)]
        [HttpGet("{id}")]
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

        // PUT: api/granjas/{id}
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAgro)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGranja(int id, [FromBody] GranjaDto modelGranja)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            if (id != modelGranja.AgroindustriaId)
            {
                return BadRequest("O ID da agroindústria no token não corresponde ao ID da agroindústria no DTO.");
            }

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

        // POST: api/granjas
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAgro)]
        [HttpPost]
        public async Task<IActionResult> PostGranja([FromBody] GranjaDto modelGranja)
        {
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            if (agroindustriaId != modelGranja.AgroindustriaId)
            {
                return BadRequest("O ID da agroindústria no token não corresponde ao ID da agroindústria no DTO.");
            }

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

        // DELETE: api/granjas/{id}
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAgro)]
        [HttpDelete("{id}")]
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
