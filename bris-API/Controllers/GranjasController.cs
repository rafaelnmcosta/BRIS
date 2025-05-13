using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using bris_API.Data;
using bris_API.Models;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    /// <summary>
    /// Controller responsável por gerenciar as operações relacionadas às granjas,
    /// como listagem, cadastro, edição, ativação e desativação.
    /// </summary>
    [Route("api/granjas")]
    [ApiController]
    public class GranjasController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Construtor que injeta o contexto do banco de dados.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public GranjasController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Obtém a lista de todas as granjas.
        /// </summary>
        /// <returns>Lista de granjas.</returns>
        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Granja>>> GetGranjas()
        {
            // Busca todas as granjas e as retorna.
            var granjas = await _context.Granjas.ToListAsync();
            return Ok(granjas);
        }

        /// <summary>
        /// Busca granjas por Agroindústria
        /// </summary>
        /// <param name="id">ID da Agroindústria</param>
        /// <returns>Lista de granjas vinculadas com detalhes completos</returns>
        [HttpGet("agro/{id}")]
        [Authorize(Policy = "VisualizaAgroindustria")]
        public async Task<IActionResult> GetGranjasPorAgroindustria(int id)
        {
            try
            {
                var granjas = await _context.Granjas
                    .Where(g => g.AgroindustriaId == id)
                    .Include(g => g.Agroindustria)
                    .Select(g => new GetGranjaDTO
                    {
                        Id = g.Id,
                        NomePropriedade = g.NomePropriedade,
                        Endereco = g.Endereco,
                        CNPJ = g.CNPJ,
                        DataCadastro = g.DataCadastro,
                        Ativo = g.Ativo,
                        Agroindustria = new GetAgroindustriaDTO
                        {
                            Id = g.Agroindustria.Id,
                            NomeFantasia = g.Agroindustria.NomeFantasia,
                            RazaoSocial = g.Agroindustria.RazaoSocial,
                            CNPJ = g.Agroindustria.CNPJ,
                            DataCadastro = g.Agroindustria.DataCadastro,
                            Ativo = g.Agroindustria.Ativo
                        }
                    })
                    .ToListAsync();

                return Ok(granjas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Erro ao buscar granjas: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtém a lista de granjas inativas.
        /// </summary>
        /// <returns>Lista de granjas inativas.</returns>
        [Authorize(Policy = "VisualizaAgroindustria")]
        [HttpGet("ativar")]
        public async Task<ActionResult<IEnumerable<Granja>>> GetGranjasInativas()
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var agroindustriaIdClaim = User.FindFirst("AgroindustriaId")?.Value;

            IQueryable<Granja> query = _context.Granjas.Where(g => !g.Ativo);

            // Se não for ADMIN, filtra pela agroindústria do usuário
            if (role != "ADMIN")
            {
                if (string.IsNullOrEmpty(agroindustriaIdClaim) || !int.TryParse(agroindustriaIdClaim, out var agroindustriaId))
                {
                    return Forbid(); // ou return BadRequest("Agroindústria inválida");
                }

                query = query.Where(g => g.AgroindustriaId == agroindustriaId);
            }

            var granjas = await query.ToListAsync();
            return Ok(granjas);
        }

        /// <summary>
        /// Ativa uma granja inativa.
        /// </summary>
        /// <param name="id">ID da granja a ser ativada.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAgroindustria")]
        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarGranja(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var agroindustriaIdClaim = User.FindFirst("AgroindustriaId")?.Value;

            var granja = await _context.Granjas
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
            }

            if (granja.Ativo)
            {
                return BadRequest("Granja já está ativa.");
            }

            // Se não for ADMIN, valida se a granja pertence à agroindústria do usuário
            if (role != "ADMIN")
            {
                if (string.IsNullOrEmpty(agroindustriaIdClaim) || !int.TryParse(agroindustriaIdClaim, out var agroindustriaId))
                {
                    return Forbid();
                }

                if (granja.AgroindustriaId != agroindustriaId)
                {
                    return Forbid("Você não tem permissão para ativar esta granja.");
                }
            }

            granja.Ativo = true;
            _context.Entry(granja).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja ativada com sucesso!" });
        }

        /// <summary>
        /// Obtém os dados de uma granja específica.
        /// </summary>
        /// <param name="id">ID da granja.</param>
        /// <returns>Dados da granja.</returns>
        [Authorize(Policy = "VisualizaAgroindustria")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Granja>> GetGranja(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var agroindustriaIdClaim = User.FindFirst("AgroindustriaId")?.Value;

            var granja = await _context.Granjas
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
            }

            // Se não for ADMIN, verificar se pertence à mesma Agroindústria
            if (role != "ADMIN")
            {
                if (string.IsNullOrEmpty(agroindustriaIdClaim) || !int.TryParse(agroindustriaIdClaim, out var agroindustriaId))
                {
                    return Forbid();
                }

                if (granja.AgroindustriaId != agroindustriaId)
                {
                    return Forbid("Você não tem permissão para visualizar esta granja.");
                }
            }

            return Ok(granja);
        }


        /// <summary>
        /// Atualiza os dados de uma granja existente.
        /// </summary>
        /// <param name="id">ID da granja a ser editada.</param>
        /// <param name="modelGranja">Dados atualizados da granja.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAgroindustria")]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> PutGranja(int id, [FromBody] AdminEditaGranjaDto modelGranja)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var agroindustriaIdClaim = User.FindFirst("AgroindustriaId")?.Value;

            var granja = await _context.Granjas.FirstOrDefaultAsync(g => g.Id == id);

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
            }

            // Se não for ADMIN, verificar se pertence à mesma Agroindústria
            if (role != "ADMIN")
            {
                if (string.IsNullOrEmpty(agroindustriaIdClaim) || !int.TryParse(agroindustriaIdClaim, out var agroindustriaId))
                {
                    return Forbid();
                }

                if (granja.AgroindustriaId != agroindustriaId)
                {
                    return Forbid("Você não tem permissão para editar esta granja.");
                }
            }

            // Verifica duplicidade de CNPJ (excluindo a granja em questão)
            if (await _context.Granjas.AnyAsync(g => g.CNPJ == modelGranja.CNPJ && g.Id != id))
            {
                return BadRequest("Já existe uma granja com esse CNPJ cadastrado!");
            }

            // Atualiza os campos
            granja.NomePropriedade = modelGranja.NomePropriedade;
            granja.Endereco = modelGranja.Endereco;
            granja.CNPJ = modelGranja.CNPJ;
            granja.Ativo = modelGranja.Ativo;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja atualizada com sucesso!" });
        }



        /// <summary>
        /// Cadastra uma nova granja.
        /// </summary>
        /// <param name="modelGranja">Dados da nova granja.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAgroindustria")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> PostGranja([FromBody] AdminEditaGranjaDto modelGranja)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var agroindustriaIdClaim = User.FindFirst("AgroindustriaId")?.Value;

            if (role != "ADMIN")
            {
                if (string.IsNullOrEmpty(agroindustriaIdClaim) || !int.TryParse(agroindustriaIdClaim, out var agroindustriaId))
                {
                    return Forbid();
                }

                if (modelGranja.AgroindustriaId != agroindustriaId)
                {
                    return Forbid("Você não pode cadastrar granjas em outra agroindústria.");
                }
            }

            // Verifica duplicidade de CNPJ
            if (await _context.Granjas.AnyAsync(g => g.CNPJ == modelGranja.CNPJ))
            {
                return BadRequest("Já existe uma granja com esse CNPJ cadastrado!");
            }

            var novaGranja = new Granja
            {
                NomePropriedade = modelGranja.NomePropriedade,
                Endereco = modelGranja.Endereco,
                CNPJ = modelGranja.CNPJ,
                AgroindustriaId = modelGranja.AgroindustriaId,
                Ativo = modelGranja.Ativo
            };

            _context.Granjas.Add(novaGranja);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja cadastrada com sucesso!" });
        }


        /// <summary>
        /// Desativa uma granja.
        /// </summary>
        /// <param name="id">ID da granja a ser desativada.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAgroindustria")]
        [HttpDelete("{id}/desativar")]
        public async Task<IActionResult> DeleteGranja(int id)
        {
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var agroindustriaIdClaim = User.FindFirst("AgroindustriaId")?.Value;

            var granja = await _context.Granjas
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
            }

            if (role != "ADMIN")
            {
                if (string.IsNullOrEmpty(agroindustriaIdClaim) || !int.TryParse(agroindustriaIdClaim, out var agroindustriaId))
                {
                    return Forbid();
                }

                if (granja.AgroindustriaId != agroindustriaId)
                {
                    return Forbid("Você não tem permissão para desativar esta granja.");
                }
            }

            granja.Ativo = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja desativada com sucesso!" });
        }

    }
}
