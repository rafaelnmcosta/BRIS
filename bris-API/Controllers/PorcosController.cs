using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bris_API.Data;
using bris_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace bris_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PorcosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PorcosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Porcos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Porco>>> GetPorcos()
        {
            return await _context.Porcos.ToListAsync();
        }

        // GET: api/Porcos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Porco>> GetPorco(int id)
        {
            var porco = await _context.Porcos.FindAsync(id);

            if (porco == null)
            {
                return NotFound();
            }

            return porco;
        }

        // POST: api/Porcos
        [HttpPost]
        public async Task<ActionResult<Porco>> PostPorco(Porco porco)
        {
            _context.Porcos.Add(porco);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPorco), new { id = porco.Id }, porco);
        }

        // PUT: api/Porcos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPorco(int id, Porco porco)
        {
            if (id != porco.Id)
            {
                return BadRequest();
            }

            _context.Entry(porco).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Porcos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePorco(int id)
        {
            var porco = await _context.Porcos.FindAsync(id);
            if (porco == null)
            {
                return NotFound();
            }

            _context.Porcos.Remove(porco);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
