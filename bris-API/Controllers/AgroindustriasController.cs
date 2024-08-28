using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

using bris_API.Data;
using bris_API.Models;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    [Route("api/agroindustrias")]
    [ApiController]
    public class AgroindustriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgroindustriasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/agroindustrias
        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoTotal)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agroindustria>>> GetAgroindustrias()
        {
            var agroindustrias = await _context.Agroindustrias.ToListAsync();
            return Ok(agroindustrias);
        }

        // GET: api/agroindustrias/ativar
        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoTotal)]
        [HttpGet("ativar")]
        public async Task<ActionResult<IEnumerable<Agroindustria>>> GetAgroindustriasInativas()
        {
            var agroindustriasInativas = await _context.Agroindustrias
                .Where(a => !a.Ativo)
                .ToListAsync();
            return Ok(agroindustriasInativas);
        }
        
        // PUT: api/agroindustrias/ativar/{id}
        [Authorize(Roles = PoliticasDeAcesso.GerenciaTotal)]
        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarAgroindustria(int id)
        {
            var agroindustria = await _context.Agroindustrias.FindAsync(id);

            if (agroindustria == null)
            {
                return NotFound("Agroindústria não encontrada.");
            }

            if (agroindustria.Ativo)
            {
                return BadRequest("Agroindústria já está ativa.");
            }

            agroindustria.Ativo = true;

            _context.Entry(agroindustria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Agroindústria ativada com sucesso!" });
        }

        // GET: api/agroindustrias/{id}
        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoTotal)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Agroindustria>> GetAgroindustria(int id)
        {
            var agroindustria = await _context.Agroindustrias.FindAsync(id);

            if (agroindustria == null)
            {
                return NotFound("Agroindústria não encontrada.");
            }

            return Ok(agroindustria);
        }

        // PUT: api/agroindustrias/{id}
        [Authorize(Roles = PoliticasDeAcesso.GerenciaTotal)]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgroindustria(int id, [FromBody] AgroindustriaDTO modelAgroindustria)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            var agroindustria = await _context.Agroindustrias.FindAsync(id);

            if (agroindustria == null)
            {
                return NotFound("Agroindústria não encontrada.");
            }

            agroindustria.NomeFantasia = modelAgroindustria.NomeFantasia;
            agroindustria.RazaoSocial = modelAgroindustria.RazaoSocial;
            agroindustria.CNPJ = modelAgroindustria.CNPJ;
            agroindustria.Ativo = modelAgroindustria.Ativo;

            _context.Entry(agroindustria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Agroindústria atualizada com sucesso!" });
        }

        // POST: api/agroindustrias
        [Authorize(Roles = PoliticasDeAcesso.GerenciaTotal)]
        [HttpPost]
        public async Task<ActionResult<Agroindustria>> PostAgroindustria([FromBody] AgroindustriaDTO modelAgroindustria)
        {
            var agroindustria = new Agroindustria
            {
                NomeFantasia = modelAgroindustria.NomeFantasia,
                RazaoSocial = modelAgroindustria.RazaoSocial,
                CNPJ = modelAgroindustria.CNPJ,
                Ativo = modelAgroindustria.Ativo
            };

            _context.Agroindustrias.Add(agroindustria);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetAgroindustria), new { id = agroindustria.Id }, agroindustria);
        }

        // DELETE: api/agroindustrias/{id}
        [Authorize(Roles = PoliticasDeAcesso.GerenciaTotal)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgroindustria(int id)
        {
            var agroindustria = await _context.Agroindustrias.FindAsync(id);

            if (agroindustria == null)
            {
                return NotFound("Agroindústria não encontrada.");
            }

            agroindustria.Ativo = false;

            _context.Entry(agroindustria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Agroindústria desativada com sucesso!" });
        }
    }
}