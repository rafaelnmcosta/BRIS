using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
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
        /// Obtém a lista de granjas inativas.
        /// </summary>
        /// <returns>Lista de granjas inativas.</returns>
        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet("ativar")]
        public async Task<ActionResult<IEnumerable<Granja>>> GetGranjasInativas()
        {
            // Filtra granjas cujo campo Ativo seja false e as retorna.
            var granjas = await _context.Granjas
                .Where(g => !g.Ativo)
                .ToListAsync();
            return Ok(granjas);
        }

        /// <summary>
        /// Ativa uma granja inativa.
        /// </summary>
        /// <param name="id">ID da granja a ser ativada.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaTotal")]
        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarGranja(int id)
        {
            // Busca a granja pelo ID.
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

            // Define a propriedade Ativo como true para ativar a granja.
            granja.Ativo = true;

            // Atualiza o estado da granja no contexto e salva as alterações.
            _context.Entry(granja).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja ativada com sucesso!" });
        }

        /// <summary>
        /// Obtém os dados de uma granja específica.
        /// </summary>
        /// <param name="id">ID da granja.</param>
        /// <returns>Dados da granja.</returns>
        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Granja>> GetGranja(int id)
        {
            // Busca a granja pelo ID.
            var granja = await _context.Granjas
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
            }

            return Ok(granja);
        }

        /// <summary>
        /// Atualiza os dados de uma granja existente.
        /// </summary>
        /// <param name="id">ID da granja a ser editada.</param>
        /// <param name="modelGranja">Dados atualizados da granja.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaTotal")]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> PutGranja(int id, [FromBody] AdminEditaGranjaDto modelGranja)
        {
            // Busca a granja a ser editada pelo ID.
            var granja = await _context.Granjas
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
            }

            // Atualiza os campos da granja com os valores fornecidos no DTO.
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
        [Authorize(Policy = "GerenciaTotal")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> PostGranja([FromBody] AdminEditaGranjaDto modelGranja)
        {
            // Cria uma nova instância de granja com os dados fornecidos.
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
        [Authorize(Policy = "GerenciaTotal")]
        [HttpDelete("{id}/desativar")]
        public async Task<IActionResult> DeleteGranja(int id)
        {
            // Busca a granja pelo ID.
            var granja = await _context.Granjas
                .Where(g => g.Id == id)
                .FirstOrDefaultAsync();

            if (granja == null)
            {
                return NotFound("Granja não encontrada.");
            }

            // Define a granja como inativa.
            granja.Ativo = false;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Granja desativada com sucesso!" });
        }
    }
}
