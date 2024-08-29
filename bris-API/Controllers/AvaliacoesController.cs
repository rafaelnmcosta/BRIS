using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using bris_API.Data;
using bris_API.Models;
using bris_API.DTOs;

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
        [Authorize(Roles = PoliticasDeAcesso.VisualizaAnimais)]
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
        [Authorize(Roles = PoliticasDeAcesso.VisualizaAnimais)]
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
        [Authorize(Roles = PoliticasDeAcesso.VisualizaAnimais)]
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
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAnimais)]
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
                DataInicioAvaliacao = DateTime.Now,
                StatusAvaliacao = 1,
                ResultadoFinal = null,
                Semanas = new List<Semana>(),
                ProximaDoseSemana = 1,
                ProximaDoseOrdem = 0
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
                    var podePreencher = i == 1 && ordem == 0;

                    var dose = new Dose
                    {
                        SemanaId = semana.Id,
                        UsuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                        DataRegistro = null,
                        ValorRegistrado = null,
                        Ordem = ordem,
                        PodePreencher = podePreencher
                    };

                    _context.Doses.Add(dose);
                }
            }

            await _context.SaveChangesAsync();

            return Ok("Avaliação e semanas criadas com sucesso.");
        }

        // PUT: api/avaliacoes/{id}/nova-dose
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAnimais)]
        [HttpPut("{id}/nova-dose")]
        public async Task<ActionResult> NovaDose(int id, [FromBody] DoseDto model)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;

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
                return Forbid("Apenas avaliações em aberto podem receber novas doses.");
            }

            var dose = avaliacao.Semanas
                .FirstOrDefault(s => s.NroSemana == avaliacao.ProximaDoseSemana)?
                .Doses.FirstOrDefault(d => d.Ordem == avaliacao.ProximaDoseOrdem);

            if (dose == null || !dose.PodePreencher)
            {
                return BadRequest("Há inconsistência nos dados.");
            }

            dose.ValorRegistrado = model.ValorRegistrado;
            dose.DataRegistro = DateTime.Now;
            dose.PodePreencher = false;

            // Atualiza a próxima dose e semana
            var avaliacaoAtualizada = AtualizaProximaDose(avaliacao);
            avaliacao.ProximaDoseSemana = avaliacaoAtualizada.ProximaDoseSemana;
            avaliacao.ProximaDoseOrdem = avaliacaoAtualizada.ProximaDoseOrdem;

            await _context.SaveChangesAsync();

            return Ok("Dose registrada com sucesso.");
        }

        // PUT: api/avaliacoes/finaliza/{id}
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAnimais)]
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
        [Authorize(Roles = PoliticasDeAcesso.GerenciaAnimais)]
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

            avaliacao.StatusAvaliacao = 2;
            await _context.SaveChangesAsync();

            return Ok("Avaliação interrompida com sucesso.");
        }

        private int GeraResultadoSemana(Semana semana)
        {
            // Obtém a dose de 120h
            var dose120h = semana.Doses.FirstOrDefault(d => d.Ordem == 1);
            
            // Obtém a dose de 168h
            var dose168h = semana.Doses.FirstOrDefault(d => d.Ordem == 2);

            //Caso uma das doses seja nula retorna zero para indicar erro
            if (dose120h == null || dose168h == null) return 0;

            // Verifica o PMP na dose de 120h
            if (dose120h.ValorRegistrado < 60)
            {
                // retorna código para "Maior" caso o PMP às 120h seja menor que 60%;
                return 3;
            }

            // Verifica o PMP na dose de 120h e na dose de 168h
            if (dose120h.ValorRegistrado >= 60 && dose168h.ValorRegistrado < 60)
            {
                // retorna código para "Médio"  caso o PMP às 120h seja maior ou igual a 60% e às 168h seja menor que 60%;
                return 2;
            }

            if (dose168h.ValorRegistrado >= 60)
            {
                // retorna código para "Menor" caso o PMP às 168h seja maior ou igual a 60%;
                return 1; 
            }

            // Retorna código para erro se nenhum dos critérios for atendido
            return 0;
        }


        private bool GeraResultadoFinal(Avaliacao avaliacao)
        {
            // Conta quantas semanas obtiveram resultado de sensibilidade "Maior"
            int semanasComResultadoMaior = avaliacao.Semanas.Count(s => s.Resultado == 3);

            // Retorna false se pelo menos 3 semanas têm resultado "Maior"
            return semanasComResultadoMaior < 3;
        }

        private Avaliacao AtualizaProximaDose(Avaliacao avaliacao)
        {
            var proximaOrdem = avaliacao.ProximaDoseOrdem + 1;

            // Caso complete as doses da semana, inicia uma nova semana e gera o resultado da semana fechada
            if (proximaOrdem > 2)
            {
                proximaOrdem = 0;
                var semanaAtual = avaliacao.Semanas
                    .FirstOrDefault(s => s.NroSemana == avaliacao.ProximaDoseSemana);

                // Verifica se a semanaAtual não é nula antes de calcular o resultado
                if (semanaAtual != null)
                {
                    semanaAtual.Resultado = GeraResultadoSemana(semanaAtual);
                }

                // Atualiza o número da semana para a próxima
                avaliacao.ProximaDoseSemana++;
            }

            // Caso complete as 5 semanas, finaliza a avaliação
            if (avaliacao.ProximaDoseSemana > 5)
            {
                avaliacao.ProximaDoseSemana = -1;
                avaliacao.ProximaDoseOrdem = -1;
                avaliacao.StatusAvaliacao = 2;
                avaliacao.ResultadoFinal = GeraResultadoFinal(avaliacao);
            }
            else
            {
                // Atualiza a ordem da dose
                avaliacao.ProximaDoseOrdem = proximaOrdem;

                // Obtém a próxima dose
                var proximaDose = avaliacao.Semanas
                    .FirstOrDefault(s => s.NroSemana == avaliacao.ProximaDoseSemana)?
                    .Doses.FirstOrDefault(d => d.Ordem == avaliacao.ProximaDoseOrdem);

                // Marca a próxima dose como preenchível, se existir
                if (proximaDose != null)
                {
                    proximaDose.PodePreencher = true;
                }
            }

            return avaliacao;
        }


    }

}