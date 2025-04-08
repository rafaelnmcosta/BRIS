using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using bris_API.Data;
using bris_API.Models;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações relacionadas às agroindústrias, incluindo listagem, cadastro, edição, ativação e desativação.
    /// </summary>
    [Route("api/agroindustrias")]
    [ApiController]
    public class AgroindustriasController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Construtor que injeta o contexto do banco de dados.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public AgroindustriasController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna a lista de todas as agroindústrias.
        /// </summary>
        /// <returns>Lista de agroindústrias.</returns>
        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Agroindustria>>> GetAgroindustrias()
        {
            // Busca todas as agroindústrias no banco de dados e retorna
            var agroindustrias = await _context.Agroindustrias.ToListAsync();
            return Ok(agroindustrias);
        }

        /// <summary>
        /// Retorna a lista de agroindústrias inativas.
        /// </summary>
        /// <returns>Lista de agroindústrias inativas.</returns>
        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet("ativar")]
        public async Task<ActionResult<IEnumerable<Agroindustria>>> GetAgroindustriasInativas()
        {
            // Filtra as agroindústrias que estão inativas (Ativo = false)
            var agroindustriasInativas = await _context.Agroindustrias
                .Where(a => !a.Ativo)
                .ToListAsync();
            return Ok(agroindustriasInativas);
        }

        /// <summary>
        /// Ativa uma agroindústria inativa.
        /// </summary>
        /// <param name="id">ID da agroindústria a ser ativada.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaTotal")]
        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarAgroindustria(int id)
        {
            // Busca a agroindústria pelo ID
            var agroindustria = await _context.Agroindustrias.FindAsync(id);

            if (agroindustria == null)
            {
                return NotFound("Agroindústria não encontrada.");
            }

            if (agroindustria.Ativo)
            {
                return BadRequest("Agroindústria já está ativa.");
            }

            // Ativa a agroindústria
            agroindustria.Ativo = true;

            // Marca a entidade como modificada e salva as alterações no banco
            _context.Entry(agroindustria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Agroindústria ativada com sucesso!" });
        }

        /// <summary>
        /// Retorna os dados de uma agroindústria específica.
        /// </summary>
        /// <param name="id">ID da agroindústria.</param>
        /// <returns>Dados da agroindústria.</returns>
        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Agroindustria>> GetAgroindustria(int id)
        {
            // Busca a agroindústria pelo ID
            var agroindustria = await _context.Agroindustrias.FindAsync(id);

            if (agroindustria == null)
            {
                return NotFound("Agroindústria não encontrada.");
            }

            return Ok(agroindustria);
        }

        /// <summary>
        /// Atualiza os dados de uma agroindústria existente.
        /// </summary>
        /// <param name="id">ID da agroindústria a ser editada.</param>
        /// <param name="modelAgroindustria">Dados atualizados da agroindústria.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaTotal")]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> PutAgroindustria(int id, [FromBody] AgroindustriaDTO modelAgroindustria)
        {
            if (id <= 0)
            {
                return BadRequest("ID inválido.");
            }

            // Busca a agroindústria a ser editada
            var agroindustria = await _context.Agroindustrias.FindAsync(id);

            if (agroindustria == null)
            {
                return NotFound("Agroindústria não encontrada.");
            }

            // Atualiza os campos da agroindústria conforme os dados recebidos
            agroindustria.NomeFantasia = modelAgroindustria.NomeFantasia;
            agroindustria.RazaoSocial = modelAgroindustria.RazaoSocial;
            agroindustria.CNPJ = modelAgroindustria.CNPJ;
            agroindustria.Ativo = modelAgroindustria.Ativo;

            // Marca a entidade como modificada e salva as alterações
            _context.Entry(agroindustria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Agroindústria atualizada com sucesso!" });
        }

        /// <summary>
        /// Cadastra uma nova agroindústria.
        /// </summary>
        /// <param name="modelAgroindustria">Dados da nova agroindústria.</param>
        /// <returns>Agroindústria criada, com status Created.</returns>
        [Authorize(Policy = "GerenciaTotal")]
        [HttpPost("cadastrar")]
        public async Task<ActionResult<Agroindustria>> PostAgroindustria([FromBody] AgroindustriaDTO modelAgroindustria)
        {
            // Cria uma nova instância de Agroindustria com os dados fornecidos
            var agroindustria = new Agroindustria
            {
                NomeFantasia = modelAgroindustria.NomeFantasia,
                RazaoSocial = modelAgroindustria.RazaoSocial,
                CNPJ = modelAgroindustria.CNPJ,
                Ativo = modelAgroindustria.Ativo
            };

            _context.Agroindustrias.Add(agroindustria);
            await _context.SaveChangesAsync();

            // Retorna status 201 Created com os dados da agroindústria recém-cadastrada
            return CreatedAtAction(nameof(GetAgroindustria), new { id = agroindustria.Id }, agroindustria);
        }

        /// <summary>
        /// Desativa uma agroindústria.
        /// </summary>
        /// <param name="id">ID da agroindústria a ser desativada.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaTotal")]
        [HttpDelete("{id}/desativar")]
        public async Task<IActionResult> DeleteAgroindustria(int id)
        {
            // Busca a agroindústria pelo ID
            var agroindustria = await _context.Agroindustrias.FindAsync(id);

            if (agroindustria == null)
            {
                return NotFound("Agroindústria não encontrada.");
            }

            // Define a agroindústria como inativa
            agroindustria.Ativo = false;

            // Marca a entidade como modificada e salva as alterações
            _context.Entry(agroindustria).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Agroindústria desativada com sucesso!" });
        }
    }
}
