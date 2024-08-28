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
    [Route("api/agroindustrias")]
    [ApiController]
    public class AgroindustriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AgroindustriasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Agroindustrias
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agroindustria>>> GetAgroindustrias()
        {
            return await _context.Agroindustrias.ToListAsync();
        }

        // GET: api/Agroindustrias/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Agroindustria>> GetAgroindustria(int id)
        {
            var agroindustria = await _context.Agroindustrias.FindAsync(id);

            if (agroindustria == null)
            {
                return NotFound();
            }

            return agroindustria;
        }

        // PUT: api/Agroindustrias/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAgroindustria(int id, Agroindustria agroindustria)
        {
            if (id != agroindustria.Id)
            {
                return BadRequest();
            }

            _context.Entry(agroindustria).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AgroindustriaExists(id))
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

        // POST: api/Agroindustrias
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Agroindustria>> PostAgroindustria(Agroindustria agroindustria)
        {
            _context.Agroindustrias.Add(agroindustria);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAgroindustria", new { id = agroindustria.Id }, agroindustria);
        }

        // DELETE: api/Agroindustrias/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAgroindustria(int id)
        {
            var agroindustria = await _context.Agroindustrias.FindAsync(id);
            if (agroindustria == null)
            {
                return NotFound();
            }

            _context.Agroindustrias.Remove(agroindustria);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool AgroindustriaExists(int id)
        {
            return _context.Agroindustrias.Any(e => e.Id == id);
        }
    }
}
