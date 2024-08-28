using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using bris_API.Data;
using bris_API.Models;
using bris_API.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

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

        // GET: api/Animais
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimais()
        {
            // Obter o ID da Agroindústria do usuário autenticado
            var agroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value);

            // Filtrar animais pela Agroindústria do usuário
            return await _context.Animais
                                .Include(a => a.Granja) // Inclua a Granja para acessar a FK
                                .Where(a => a.Granja.AgroindustriaId == agroindustriaId)
                                .ToListAsync();
        }


        // GET: api/Animais/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var animal = await _context.Animais.FindAsync(id);

            if (animal == null)
            {
                return NotFound();
            }

            return animal;
        }

        // POST: api/Animais
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAnimais)]
        [HttpPost]
        public async Task<IActionResult> CadastrarAnimal([FromBody] CadastroAnimalDto animalDTO)
        {
            // Obter o ID do usuário autenticado
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var granjaId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            // Realizar a busca pelo usuário e a granja associada
            var usuarioResponsavel = await _context.Usuarios.FindAsync(int.Parse(userId));
            var granjaAssociada = await _context.Granjas.FindAsync(int.Parse(granjaId));

            if (usuarioResponsavel == null || granjaAssociada == null)
            {
                return NotFound("Usuário ou granja não encontrados.");
            }

            // Criar o objeto Animal
            var animal = new Animal
            {
                Linhagem = animalDTO.Linhagem,
                Idade = animalDTO.Idade,
                Peso = animalDTO.Peso,
                Usuario = usuarioResponsavel,
                Granja = granjaAssociada
            };

            // Salvar no banco de dados
            _context.Animais.Add(animal);
            await _context.SaveChangesAsync();

            return Ok(animal);
        }

        // PUT: api/Animais/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAnimal(int id, Animal animal)
        {
            if (id != animal.Id)
            {
                return BadRequest();
            }

            _context.Entry(animal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // DELETE: api/Animais/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var animal = await _context.Animais.FindAsync(id);
            if (animal == null)
            {
                return NotFound();
            }

            _context.Animais.Remove(animal);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
