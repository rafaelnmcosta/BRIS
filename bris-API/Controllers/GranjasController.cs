using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bris_API.Data;
using bris_API.Models;

namespace bris_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GranjasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public GranjasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Granjas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Granja>>> GetGranjas()
        {
            return await _context.Granjas.ToListAsync();
        }

        // GET: api/Granjas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Granja>> GetGranja(int id)
        {
            var granja = await _context.Granjas.FindAsync(id);

            if (granja == null)
            {
                return NotFound();
            }

            return granja;
        }

        // PUT: api/Granjas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGranja(int id, Granja granja)
        {
            if (id != granja.Id)
            {
                return BadRequest();
            }

            _context.Entry(granja).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GranjaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Granjas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Granja>> PostGranja(Granja granja)
        {
            _context.Granjas.Add(granja);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGranja", new { id = granja.Id }, granja);
        }

        // DELETE: api/Granjas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGranja(int id)
        {
            var granja = await _context.Granjas.FindAsync(id);
            if (granja == null)
            {
                return NotFound();
            }

            _context.Granjas.Remove(granja);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GranjaExists(int id)
        {
            return _context.Granjas.Any(e => e.Id == id);
        }
    }
}
