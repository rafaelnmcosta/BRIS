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
    /// <summary>
    /// Controller responsável pelas operações relacionadas às avaliações dos animais.
    /// Inclui a consulta de avaliações (ativas, interrompidas e por ID), criação de nova avaliação,
    /// registro de doses, finalização, interrupção e reativação de avaliações.
    /// </summary>
    [Route("api/avaliacoes")]
    [ApiController]
    public class AvaliacoesController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Construtor que injeta o contexto do banco de dados.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public AvaliacoesController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/avaliacoes
        /// Retorna a lista de avaliações associadas aos animais que pertencem à granja do usuário autenticado,
        /// excluindo aquelas com StatusAvaliacao igual a 3 (finalizadas).
        /// </summary>
        /// <returns>Lista de avaliações ativas.</returns>
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet()]
        public async Task<ActionResult<IEnumerable<Avaliacao>>> GetAvaliacoes()
        {
            // Recupera o ID da granja a partir da claim "GranjaId"
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca as avaliações cujo animal pertence à granja e cuja avaliação não está finalizada (StatusAvaliacao != 3)
            var avaliacoes = await _context.Avaliacoes
                .Include(a => a.Animal)
                .Where(a => a.Animal.Granja.Id == int.Parse(granjaId) && a.StatusAvaliacao != 3)
                .ToListAsync();

            return Ok(avaliacoes);
        }

        /// <summary>
        /// GET: api/avaliacoes/interrompidas
        /// Retorna a lista de avaliações interrompidas (StatusAvaliacao == 3) associadas aos animais da granja do usuário autenticado.
        /// </summary>
        /// <returns>Lista de avaliações interrompidas.</returns>
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("interrompidas")]
        public async Task<ActionResult<IEnumerable<Avaliacao>>> GetAvaliacoesInterrompidas()
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca avaliações interrompidas (StatusAvaliacao == 3)
            var avaliacoesInterrompidas = await _context.Avaliacoes
                .Include(a => a.Animal)
                .Where(a => a.Animal.Granja.Id == int.Parse(granjaId) && a.StatusAvaliacao == 3)
                .ToListAsync();

            return Ok(avaliacoesInterrompidas);
        }

        /// <summary>
        /// GET: api/avaliacoes/{id}
        /// Retorna os detalhes de uma avaliação específica, garantindo que ela pertença à granja do usuário autenticado.
        /// </summary>
        /// <param name="id">ID da avaliação.</param>
        /// <returns>Dados da avaliação.</returns>
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Avaliacao>> GetAvaliacao(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca a avaliação pelo ID, incluindo o animal e as semanas associadas, filtrando pela granja do usuário
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

        /// <summary>
        /// POST: api/avaliacoes/nova/{id}
        /// Cria uma nova avaliação para um animal específico, identificando o animal pela ID fornecida e garantindo que ele pertence à granja do usuário autenticado.
        /// Após criar a avaliação, são criadas 5 semanas e 3 doses para cada semana.
        /// A primeira dose da primeira semana é marcada como preenchível.
        /// </summary>
        /// <param name="id">ID do animal.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPost("nova/{id}")]
        public async Task<ActionResult> NovaAvaliacao(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca o animal, garantindo que ele pertence à granja do usuário autenticado.
            var animal = await _context.Animais
                .Include(a => a.Granja)
                .FirstOrDefaultAsync(a => a.Id == id && a.Granja.Id == int.Parse(granjaId));

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua granja.");
            }

            // Cria uma nova avaliação para o animal
            var avaliacao = new Avaliacao
            {
                AnimalId = id,
                DataInicioAvaliacao = DateTime.UtcNow,
                StatusAvaliacao = 1, // 1 indica avaliação aberta
                ResultadoFinal = null,
                Semanas = new List<Semana>(),
                ProximaDoseSemana = 1,
                ProximaDoseOrdem = 1
            };

            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

            // Cria 5 semanas para a avaliação, cada uma com 3 doses.
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

                // Cria 3 doses para a semana
                for (int ordem = 1; ordem <= 3; ordem++)
                {
                    // Apenas a primeira dose da primeira semana poderá ser preenchida
                    var dose = new Dose
                    {
                        SemanaId = semana.Id,
                        UsuarioId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value),
                        DataRegistro = null,
                        ValorRegistrado = null,
                        Ordem = ordem,
                        PodePreencher = (i == 1 && ordem == 1)
                    };

                    _context.Doses.Add(dose);
                }
            }

            await _context.SaveChangesAsync();

            return Ok("Avaliação e semanas criadas com sucesso.");
        }

        /// <summary>
        /// PUT: api/avaliacoes/{id}/nova-dose
        /// Registra uma nova dose para uma avaliação, utilizando os dados enviados no DTO.
        /// Atualiza a dose correspondente (se preenchível) e atualiza os indicadores de próxima dose na avaliação.
        /// </summary>
        /// <param name="id">ID da avaliação.</param>
        /// <param name="model">Dados da dose a ser registrada.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("{id}/nova-dose")]
        public async Task<ActionResult> NovaDose(int id, [FromBody] DoseDto model)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            // Instancia o serviço de resultados para processamento da avaliação
            var resultsService = new ResultsService();

            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca a avaliação, incluindo o animal e as semanas/doses associadas, filtrando pela granja do usuário
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

            // Localiza a dose que deve ser preenchida com base na semana e ordem definidos na avaliação
            var dose = avaliacao.Semanas
                .FirstOrDefault(s => s.NroSemana == avaliacao.ProximaDoseSemana)?
                .Doses.FirstOrDefault(d => d.Ordem == avaliacao.ProximaDoseOrdem);

            if (dose == null || !dose.PodePreencher)
            {
                return BadRequest("Há inconsistência nos dados.");
            }

            // Atualiza a dose com os dados recebidos
            dose.ValorRegistrado = model.ValorRegistrado;
            dose.DataRegistro = DateTime.UtcNow;
            dose.PodePreencher = false;

            // Processa a avaliação para determinar os próximos indicadores de dose
            var avaliacaoAtualizada = resultsService.ProcessaAvaliacao(avaliacao);
            avaliacao.ProximaDoseSemana = avaliacaoAtualizada.ProximaDoseSemana;
            avaliacao.ProximaDoseOrdem = avaliacaoAtualizada.ProximaDoseOrdem;

            Console.Write("\n\n Proxima dose atualizada!");
            Console.Write("\n\n A proxima semana é: " + avaliacao.ProximaDoseSemana);
            Console.Write("\n\n A proxima dose é: " + avaliacao.ProximaDoseOrdem);

            await _context.SaveChangesAsync();

            return Ok("Dose registrada com sucesso.");
        }

        /// <summary>
        /// PUT: api/avaliacoes/finaliza/{id}
        /// Finaliza uma avaliação, alterando seu status para 3.
        /// </summary>
        /// <param name="id">ID da avaliação a ser finalizada.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("finaliza/{id}")]
        public async Task<ActionResult> FinalizaAvaliacao(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca a avaliação, incluindo o animal, garantindo que o animal pertence à granja do usuário autenticado
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

            // Define o status da avaliação como finalizada (3)
            avaliacao.StatusAvaliacao = 3;
            await _context.SaveChangesAsync();

            return Ok("Avaliação finalizada com sucesso.");
        }

        /// <summary>
        /// PUT: api/avaliacoes/interrompe/{id}
        /// Interrompe uma avaliação (alterando seu status para 3), caso não esteja já finalizada ou interrompida.
        /// </summary>
        /// <param name="id">ID da avaliação a ser interrompida.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("interrompe/{id}")]
        public async Task<ActionResult> InterrompeAvaliacao(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca a avaliação, incluindo o animal, garantindo que pertence à granja do usuário autenticado
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

            // Altera o status da avaliação para 3 (interrompida/finalizada)
            avaliacao.StatusAvaliacao = 3;
            await _context.SaveChangesAsync();

            return Ok("Avaliação interrompida com sucesso.");
        }

        /// <summary>
        /// PUT: api/avaliacoes/{id}/reativar
        /// Reativa uma avaliação que está em aberto (StatusAvaliacao diferente de 2 ou 3).
        /// </summary>
        /// <param name="id">ID da avaliação a ser reativada.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("{id}/reativar")]
        public async Task<ActionResult> ReativaAvaliacao(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca a avaliação, incluindo o animal, garantindo que pertence à granja do usuário autenticado
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

            // Altera o status para 1, reativando a avaliação
            avaliacao.StatusAvaliacao = 1;
            await _context.SaveChangesAsync();

            return Ok("Avaliação reativada com sucesso.");
        }
    }
}
