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
        /// GET: api/avaliacoes/granja/{granjaId}
        /// Retorna a lista de avaliações associadas aos animais que pertencem a uma granja específica.
        /// </summary>
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("granja/{granjaId}")]
        public async Task<ActionResult<IEnumerable<GetAvaliacaoDTO>>> GetAvaliacoesPorGranja(int granjaId)
        {
            // Claims do usuário
            var userRole = User.FindFirst("Role")?.Value ?? string.Empty;
            var agroIdClaim = int.TryParse(User.FindFirst("AgroindustriaId")?.Value, out var agroId) ? agroId : (int?)null;
            var granjaIdClaim = int.TryParse(User.FindFirst("GranjaId")?.Value, out var granjaIdUser) ? granjaIdUser : (int?)null;

            // Valida permissões
            if (userRole is "GESTOR_GRANJA" or "TECNICO")
            {
                if (granjaIdClaim == null || granjaIdClaim != granjaId)
                    return Forbid();
            }
            else if (userRole == "GESTOR_AGRO")
            {
                if (agroIdClaim == null)
                    return Forbid();

                var granja = await _context.Granjas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(g => g.Id == granjaId);

                if (granja == null)
                    return NotFound("Granja não encontrada.");

                if (granja.AgroindustriaId != agroIdClaim)
                    return Forbid();
            }
            else if (userRole != "ADMIN")
            {
                return Forbid();
            }

            // Busca otimizada: pega só os campos necessários
            var avaliacoes = await _context.Avaliacoes
                .Where(a => a.Animal.GranjaId == granjaId)
                .Select(a => new GetAvaliacaoDTO
                {
                    Id = a.Id,
                    AnimalId = a.AnimalId,
                    Linhagem = a.Animal.Linhagem,
                    DataInicioAvaliacao = a.DataInicioAvaliacao,
                    StatusAvaliacao = a.StatusAvaliacao,
                    ResultadoFinal = a.ResultadoFinal
                })
                .AsNoTracking()
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
        /// Retorna os detalhes de uma avaliação específica, garantindo que o usuário tenha permissão de acesso.
        /// </summary>
        /// <param name="id">ID da avaliação.</param>
        /// <returns>Dados detalhados da avaliação.</returns>
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAvaliacaoDetalhadaDTO>> GetAvaliacao(int id)
        {
            // Claims do usuário
            var userRole = User.FindFirst("Role")?.Value ?? string.Empty;
            var agroIdClaim = User.FindFirst("AgroindustriaId")?.Value;
            var granjaIdClaim = User.FindFirst("GranjaId")?.Value;

            // Busca a avaliação com animal, granja, semanas e doses
            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Animal)
                    .ThenInclude(animal => animal.Granja)
                .Include(a => a.Semanas)
                    .ThenInclude(s => s.Doses)
                        .ThenInclude(d => d.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (avaliacao == null)
                return NotFound("Avaliação não encontrada.");

            // Validações por role
            if (userRole == "GESTOR_GRANJA" || userRole == "TECNICO")
            {
                if (string.IsNullOrEmpty(granjaIdClaim) || avaliacao.Animal.GranjaId != int.Parse(granjaIdClaim))
                    return Forbid("Você não tem permissão para acessar esta avaliação.");
            }
            else if (userRole == "GESTOR_AGRO")
            {
                if (string.IsNullOrEmpty(agroIdClaim))
                    return Forbid("Agroindústria do usuário não identificada.");

                if (avaliacao.Animal.Granja.AgroindustriaId != int.Parse(agroIdClaim))
                    return Forbid("Esta avaliação não pertence à sua agroindústria.");
            }
            else if (userRole != "ADMIN")
            {
                return Forbid("Você não tem permissão para acessar esta avaliação.");
            }

            // Mapeamento para DTO detalhado
            var avaliacaoDTO = new GetAvaliacaoDetalhadaDTO
            {
                Id = avaliacao.Id,
                AnimalId = avaliacao.AnimalId,
                Linhagem = avaliacao.Animal.Linhagem,
                DataInicioAvaliacao = avaliacao.DataInicioAvaliacao,
                StatusAvaliacao = avaliacao.StatusAvaliacao.ToString(),
                ResultadoFinal = avaliacao.ResultadoFinal.HasValue ? avaliacao.ResultadoFinal.ToString() : null,
                Semanas = avaliacao.Semanas.Select(s => new GetSemanaDTO
                {
                    Id = s.Id,
                    NroSemana = s.NroSemana,
                    Resultado = s.Resultado,
                    Doses = s.Doses.Select(d => new GetDoseDTO
                    {
                        Id = d.Id,
                        SemanaId = d.SemanaId,
                        UsuarioId = d.UsuarioId,
                        Usuario = d.Usuario != null ? new GetUsuarioDTO
                        {
                            Id = d.Usuario.Id,
                            Nome = d.Usuario.Nome,
                            Email = d.Usuario.Email
                        } : null,
                        DataRegistro = d.DataRegistro,
                        ValorRegistrado = d.ValorRegistrado,
                        Ordem = d.Ordem,
                        PodePreencher = d.PodePreencher
                    }).ToList()
                }).ToList()
            };

            return Ok(avaliacaoDTO);
        }

        /// <summary>
        /// POST: api/avaliacoes/nova/{id}
        /// Cria uma nova avaliação para um animal específico, garantindo permissões baseadas na role do usuário.
        /// Cria 5 semanas e 3 doses para cada semana, com a primeira dose da primeira semana preenchível.
        /// </summary>
        /// <param name="id">ID do animal.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPost("nova/{id}")]
        public async Task<ActionResult> NovaAvaliacao(int id)
        {
            var userRole = User.FindFirst("Role")?.Value ?? string.Empty;
            var granjaIdClaim = User.FindFirst("GranjaId")?.Value;
            var agroIdClaim = User.FindFirst("AgroindustriaId")?.Value;
            var usuarioIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(usuarioIdClaim))
                return Unauthorized("Usuário não identificado.");

            int usuarioId = int.Parse(usuarioIdClaim);

            // Busca o animal e inclui a granja
            var animal = await _context.Animais
                .Include(a => a.Granja)
                .ThenInclude(g => g.Agroindustria)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
                return NotFound("Animal não encontrado.");

            // Validações de acesso
            if (userRole == "GESTOR_GRANJA" || userRole == "TECNICO")
            {
                if (string.IsNullOrEmpty(granjaIdClaim) || animal.GranjaId != int.Parse(granjaIdClaim))
                    return Forbid("Você não tem permissão para criar avaliação para este animal.");
            }
            else if (userRole == "GESTOR_AGRO")
            {
                if (string.IsNullOrEmpty(agroIdClaim) || animal.Granja.AgroindustriaId != int.Parse(agroIdClaim))
                    return Forbid("Este animal não pertence à sua agroindústria.");
            }
            // ADMIN não precisa de validação

            // Cria avaliação em memória
            var avaliacao = new Avaliacao
            {
                AnimalId = animal.Id,
                DataInicioAvaliacao = DateTime.UtcNow,
                StatusAvaliacao = 1,
                ResultadoFinal = null,
                Semanas = new List<Semana>(),
                ProximaDoseSemana = 1,
                ProximaDoseOrdem = 1
            };

            // Cria 5 semanas para a avaliação, cada uma com 3 doses.
            for (int i = 1; i <= 5; i++)
            {
                var semana = new Semana
                {
                    NroSemana = i,
                    Resultado = -1,
                    Doses = new List<Dose>()
                };

                // Cria 3 doses para a semana
                for (int ordem = 1; ordem <= 3; ordem++)
                {
                    // Apenas a primeira dose da primeira semana poderá ser preenchida
                    var dose = new Dose
                    {
                        UsuarioId = usuarioId,
                        Ordem = ordem,
                        PodePreencher = (i == 1 && ordem == 1)
                    };
                    semana.Doses.Add(dose);
                }

                avaliacao.Semanas.Add(semana);
            }

            _context.Avaliacoes.Add(avaliacao);
            await _context.SaveChangesAsync();

            return Ok("Avaliação e semanas criadas com sucesso.");
        }

        /// <summary>
        /// PUT: api/avaliacoes/{id}/nova-dose
        /// Registra uma nova dose para uma avaliação, utilizando os dados enviados no DTO.
        /// Atualiza a dose correspondente (se preenchível) e atualiza os indicadores de próxima dose na avaliação.
        /// </summary>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("{id}/nova-dose")]
        public async Task<ActionResult> NovaDose(int id, [FromBody] DoseDto model)
        {
            // Valida payload básico
            if (model == null)
                return BadRequest(new { status = "error", message = "Dados da dose não informados." });

            // Ex.: DoseDto deve conter ValorRegistrado (float?)
            if (!model.ValorRegistrado.HasValue)
                return BadRequest(new { status = "error", message = "Valor registrado é obrigatório." });

            // Parse seguro das claims
            var userRole = User.FindFirst("Role")?.Value ?? string.Empty;
            var granjaIdClaim = User.FindFirst("GranjaId")?.Value;
            int? granjaIdUser = int.TryParse(granjaIdClaim, out var tmpGranja) ? tmpGranja : (int?)null;
            var usuarioIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (!int.TryParse(usuarioIdClaim, out var usuarioId))
                return Unauthorized(new { status = "error", message = "Usuário não identificado." });

            // Busca a avaliação com animal, semanas e doses (carregando na memória)
            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Animal)
                    .ThenInclude(an => an.Granja)
                .Include(a => a.Semanas)
                    .ThenInclude(s => s.Doses)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (avaliacao == null)
                return NotFound(new { status = "error", message = "Avaliação não encontrada." });

            if (userRole == "GESTOR_GRANJA" || userRole == "TECNICO")
            {
                if (granjaIdUser == null || avaliacao.Animal.GranjaId != granjaIdUser.Value)
                    return Forbid();
            }
            else if (userRole == "GESTOR_AGRO")
            {
                var agroIdClaim = User.FindFirst("AgroindustriaId")?.Value;
                if (!int.TryParse(agroIdClaim, out var agroIdUser))
                    return Forbid();

                // certificar que a granja do animal pertence à agro do gestor
                var granjaDoAnimal = await _context.Granjas
                    .AsNoTracking()
                    .FirstOrDefaultAsync(g => g.Id == avaliacao.Animal.GranjaId);
                if (granjaDoAnimal == null || granjaDoAnimal.AgroindustriaId != agroIdUser)
                    return Forbid();
            }
            else if (userRole != "ADMIN")
            {
                return Forbid();
            }

            // Só autoriza operações em avaliações abertas (status == 1)
            if (avaliacao.StatusAvaliacao != 1)
                return BadRequest(new { status = "error", message = "Apenas avaliações em aberto podem receber novas doses." });

            // Localiza a dose esperada com base em ProximaDoseSemana/Ordem
            var semanaEsperada = avaliacao.Semanas.FirstOrDefault(s => s.NroSemana == avaliacao.ProximaDoseSemana);
            var dose = semanaEsperada?.Doses.FirstOrDefault(d => d.Ordem == avaliacao.ProximaDoseOrdem);

            if (dose == null)
                return BadRequest(new { status = "error", message = "Dose esperada não encontrada (consulte configuração da avaliação)." });

            if (!dose.PodePreencher)
                return BadRequest(new { status = "error", message = "Esta dose não está disponível para preenchimento." });

            // Evita re-gravação se já preenchida (proteção adicional)
            if (dose.ValorRegistrado.HasValue)
                return BadRequest(new { status = "error", message = "Esta dose já foi preenchida." });

            // Atualiza a dose
            dose.ValorRegistrado = model.ValorRegistrado;
            dose.DataRegistro = DateTime.UtcNow;
            dose.UsuarioId = usuarioId; // atualiza quem registrou (se quiser manter histórico)
            dose.PodePreencher = false;

            // Processa avaliação (gera resultado/atualiza próximas doses)
            var resultsService = new ResultsService(); // manter como antes; ideal injetar via DI
            var avaliacaoAtualizada = resultsService.ProcessaAvaliacao(avaliacao);

            // Opcional: validações pós-processamento (ex.: se avaliação foi finalizada, setar campos adequadamente)
            // salvamento atômico
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                // logar ex se quiser (logger)
                return StatusCode(StatusCodes.Status500InternalServerError, new { status = "error", message = "Erro ao salvar dose no banco." });
            }

            // Retorna informação útil para o front (próxima dose)
            return Ok(new
            {
                status = "success",
                message = "Dose registrada com sucesso.",
                proximaDoseSemana = avaliacaoAtualizada.ProximaDoseSemana,
                proximaDoseOrdem = avaliacaoAtualizada.ProximaDoseOrdem,
                avaliacaoFinalizada = avaliacaoAtualizada.StatusAvaliacao != 1,
                resultadoFinal = avaliacaoAtualizada.ResultadoFinal
            });
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
            var usuarioRole = User.FindFirst("Role")?.Value;
            var granjaIdClaim = User.FindFirst("GranjaId")?.Value;
            var agroIdClaim = User.FindFirst("AgroindustriaId")?.Value;

            // Busca a avaliação incluindo Animal -> Granja -> Agroindústria
            var avaliacao = await _context.Avaliacoes
                .Include(a => a.Animal)
                .ThenInclude(animal => animal.Granja)
                .ThenInclude(granja => granja.Agroindustria)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (avaliacao == null)
                return NotFound("Avaliação não encontrada.");

            // Controle de acesso baseado em role
            switch (usuarioRole)
            {
                case "admin":
                    // Admin tem acesso total, não valida nada
                    break;

                case "gestor_agro":
                    if (agroIdClaim == null || avaliacao.Animal.Granja.AgroindustriaId != int.Parse(agroIdClaim))
                        return Forbid("Você não tem permissão para finalizar avaliações dessa agroindústria.");
                    break;

                case "gestor_granja":
                case "tecnico":
                    if (granjaIdClaim == null || avaliacao.Animal.GranjaId != int.Parse(granjaIdClaim))
                        return Forbid("Você não tem permissão para finalizar avaliações desta granja.");
                    break;

                default:
                    return Forbid("Sua role não permite finalizar avaliações.");
            }

            // Verifica se já está finalizada
            if (avaliacao.StatusAvaliacao == 3)
                return BadRequest("Avaliação já está finalizada.");

            // Finaliza a avaliação
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
