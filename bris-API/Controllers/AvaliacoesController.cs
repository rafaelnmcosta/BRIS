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
    [Route("api/avaliacoes")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AvaliacoesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/avaliacoes
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Avaliacao>>> GetAvaliacoes()
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;

            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            var avaliacoes = await _context.Avaliacoes
                .Include(a => a.Animal)
                .Where(a => a.Animal.Granja.Id == int.Parse(granjaId) && a.StatusAvaliacao != 3)
                .ToListAsync();

            return Ok(avaliacoes);
        }

        // GET: api/avaliacoes/interrompidas
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("interrompidas")]
        public async Task<ActionResult<IEnumerable<Avaliacao>>> GetAvaliacoesInterrompidas()
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;

            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            var avaliacoesInterrompidas = await _context.Avaliacoes
                .Include(a => a.Animal)
                .Where(a => a.Animal.Granja.Id == int.Parse(granjaId) && a.StatusAvaliacao == 3)
                .ToListAsync();

            return Ok(avaliacoesInterrompidas);
        }

        // GET: api/avaliacoes/{id}
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Avaliacao>> GetAvaliacao(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;

            if (granjaId == null )
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Animal)
                .Include(a => a.Semanas)
                .FirstOrDefaultAsync(a => a.Id == id && a.Animal.Granja.Id == int.Parse(granjaId));

            if (avaliacao == null)
            {
                return NotFound("Avaliação não encontrada ou não pertence à sua granja.");
            }

            return Ok(avaliacao);
        }

        // POST: api/avaliacoes/nova/{id}
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPost("nova/{id}")]
        public async Task<ActionResult> NovaAvaliacao(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;

            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            var animal = await _context.Animais
                .Include(a => a.Granja)
                .FirstOrDefaultAsync(a => a.Id == id && a.Granja.Id == int.Parse(granjaId));

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua granja.");
            }

            var avaliacao = new Avaliacao
            {
                AnimalId = id,
                DataInicioAvaliacao = DateTime.UtcNow,
                StatusAvaliacao = 1,
                ResultadoFinal = null,
                Semanas = new List<Semana>(),
                ProximaDoseSemana = 1,
                ProximaDoseOrdem = 1
            };

            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

            for (int i = 1; i <= 5; i++)
            {
                var semana = new Semana
                {
                    NroSemana = i,
                    Resultado = -1,
                    AvaliacaoId = avaliacao.Id,
                    Doses = new List<Dose>()
                };

                _context.Semanas.Add(semana);
                await _context.SaveChangesAsync();

                for (int ordem = 1; ordem <= 3; ordem++)
                {
                    var podePreencher = i == 1 && ordem == 1;

                    var dose = new Dose
                    {
                        SemanaId = semana.Id,
                        UsuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                        DataRegistro = null,
                        ValorRegistrado = null,
                        Ordem = ordem,
                        PodePreencher = false
                    };

                    _context.Doses.Add(dose);
                }
            }

            // Define que a primeira dose pode ser preenchida
            avaliacao.Semanas.FirstOrDefault().Doses.FirstOrDefault().PodePreencher = true;

            await _context.SaveChangesAsync();

            return Ok("Avaliação e semanas criadas com sucesso.");
        }

        // PUT: api/avaliacoes/{id}/nova-dose
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("{id}/nova-dose")]
        public async Task<ActionResult> NovaDose(int id, [FromBody] DoseDto model)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            var resultsService = new ResultsService();

            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Animal)
                .Include(a => a.Semanas)
                .ThenInclude(s => s.Doses)
                .FirstOrDefaultAsync(a => a.Id == id && a.Animal.Granja.Id == int.Parse(granjaId));

            if (avaliacao == null)
            {
                return NotFound("Avaliação não encontrada ou não pertence à sua granja.");
            }

            if (avaliacao.StatusAvaliacao != 1)
            {
                return BadRequest("Apenas avaliações em aberto podem receber novas doses.");
            }

            var dose = avaliacao.Semanas
                .FirstOrDefault(s => s.NroSemana == avaliacao.ProximaDoseSemana)?
                .Doses.FirstOrDefault(d => d.Ordem == avaliacao.ProximaDoseOrdem);


            if (dose == null || !dose.PodePreencher)
            {
                return BadRequest("Há inconsistência nos dados.");
            }

            dose.ValorRegistrado = model.ValorRegistrado;
            dose.DataRegistro = DateTime.UtcNow;
            dose.PodePreencher = false;

            // Atualiza a próxima dose e semana

            var avaliacaoAtualizada = resultsService.ProcessaAvaliacao(avaliacao);
            avaliacao.ProximaDoseSemana = avaliacaoAtualizada.ProximaDoseSemana;
            avaliacao.ProximaDoseOrdem = avaliacaoAtualizada.ProximaDoseOrdem;

            Console.Write("\n\n Proxima dose atualizada!");
            Console.Write("\n\n A proxima semana é: " + avaliacao.ProximaDoseSemana);
            Console.Write("\n\n A proxima dose é: " + avaliacao.ProximaDoseOrdem);

            await _context.SaveChangesAsync();

            return Ok("Dose registrada com sucesso.");
        }

        // PUT: api/avaliacoes/finaliza/{id}
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("finaliza/{id}")]
        public async Task<ActionResult> FinalizaAvaliacao(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(a => a.Id == id && a.Animal.Granja.Id == int.Parse(granjaId));

            if (avaliacao == null)
            {
                return NotFound("Avaliação não encontrada ou não pertence à sua granja.");
            }

            if (avaliacao.StatusAvaliacao == 3)
            {
                return BadRequest("Avaliação já está finalizada.");
            }

            avaliacao.StatusAvaliacao = 3;
            await _context.SaveChangesAsync();

            return Ok("Avaliação finalizada com sucesso.");
        }

        // PUT: api/avaliacoes/interrompe/{id}
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("interrompe/{id}")]
        public async Task<ActionResult> InterrompeAvaliacao(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(a => a.Id == id && a.Animal.Granja.Id == int.Parse(granjaId));

            if (avaliacao == null)
            {
                return NotFound("Avaliação não encontrada ou não pertence à sua granja.");
            }

            if (avaliacao.StatusAvaliacao == 2 || avaliacao.StatusAvaliacao == 3)
            {
                return BadRequest("Avaliação já está interrompida ou finalizada.");
            }

            avaliacao.StatusAvaliacao = 3;
            await _context.SaveChangesAsync();

            return Ok("Avaliação interrompida com sucesso.");
        }

        // PUT: api/avaliacoes/{id}/reativar
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("{id}/reativar")]
        public async Task<ActionResult> ReativaAvaliacao(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Animal)
                .FirstOrDefaultAsync(a => a.Id == id && a.Animal.Granja.Id == int.Parse(granjaId));

            if (avaliacao == null)
            {
                return NotFound("Avaliação não encontrada ou não pertence à sua granja.");
            }

            if (avaliacao.StatusAvaliacao == 2 || avaliacao.StatusAvaliacao == 3)
            {
                return BadRequest("Avaliação já está interrompida ou finalizada.");
            }

            avaliacao.StatusAvaliacao = 1;
            await _context.SaveChangesAsync();

            return Ok("Avaliação interrompida com sucesso.");
        }


    }

}