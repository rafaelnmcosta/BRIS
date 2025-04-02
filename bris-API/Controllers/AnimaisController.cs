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
    /// Controller responsável pelas operações relacionadas aos animais.
    /// Inclui listagem (ativos/inativos), cadastro, edição, ativação e desativação.
    /// </summary>
    [Route("api/animais")]
    [ApiController]
    public class AnimaisController : ControllerBase
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Construtor que injeta o contexto do banco de dados.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        public AnimaisController(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retorna a lista de animais ativos pertencentes à granja definida na claim "GranjaId".
        /// </summary>
        /// <returns>Lista de animais ativos.</returns>
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimais()
        {
            // Recupera o valor da claim "GranjaId" do usuário autenticado
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca os animais ativos (Ativo = true) que pertencem à granja do usuário autenticado
            var animais = await _context.Animais
                .Where(a => a.GranjaId == int.Parse(granjaId) && a.Ativo)
                .ToListAsync();

            return Ok(animais);
        }

        /// <summary>
        /// Retorna a lista de animais inativos pertencentes à granja definida na claim "GranjaId".
        /// </summary>
        /// <returns>Lista de animais inativos.</returns>
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("ativar")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetAnimaisInativos()
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Filtra os animais inativos (Ativo = false) da granja do usuário autenticado
            var animais = await _context.Animais
                .Where(a => a.GranjaId == int.Parse(granjaId) && !a.Ativo)
                .ToListAsync();

            return Ok(animais);
        }

        /// <summary>
        /// Ativa um animal inativo pertencente à granja do usuário autenticado.
        /// </summary>
        /// <param name="id">ID do animal a ser ativado.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarAnimal(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca o animal pelo ID e garante que ele pertença à granja do usuário autenticado
            var animal = await _context.Animais
                .Where(a => a.Id == id && a.GranjaId == int.Parse(granjaId))
                .FirstOrDefaultAsync();

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua granja.");
            }

            if (animal.Ativo)
            {
                return BadRequest("Animal já está ativo.");
            }

            // Ativa o animal
            animal.Ativo = true;
            _context.Entry(animal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal ativado com sucesso!" });
        }

        /// <summary>
        /// Retorna os dados de um animal específico pertencente à granja do usuário autenticado.
        /// </summary>
        /// <param name="id">ID do animal.</param>
        /// <returns>Dados do animal.</returns>
        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Animal>> GetAnimal(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca o animal pelo ID e filtra pela granja do usuário autenticado
            var animal = await _context.Animais
                .Where(a => a.Id == id && a.GranjaId == int.Parse(granjaId))
                .FirstOrDefaultAsync();

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua granja.");
            }

            return Ok(animal);
        }

        /// <summary>
        /// Atualiza os dados de um animal, incluindo informações como linhagem, idade, peso, status e ativo.
        /// Apenas animais pertencentes à granja do usuário autenticado podem ser editados.
        /// </summary>
        /// <param name="id">ID do animal a ser editado.</param>
        /// <param name="modelAnimal">Dados para atualização do animal.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> PutAnimal(int id, [FromBody] AnimalDto modelAnimal)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca o animal pelo ID e garante que pertence à granja do usuário autenticado
            var animal = await _context.Animais
                .Where(a => a.Id == id && a.GranjaId == int.Parse(granjaId))
                .FirstOrDefaultAsync();

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua granja.");
            }

            // Atualiza os campos do animal conforme os dados recebidos no DTO
            animal.Linhagem = modelAnimal.Linhagem;
            animal.Idade = modelAnimal.Idade;
            animal.Peso = modelAnimal.Peso;
            animal.Status = modelAnimal.status;
            animal.Ativo = modelAnimal.Ativo;

            _context.Entry(animal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal atualizado com sucesso!" });
        }

        /// <summary>
        /// Cadastra um novo animal, associando-o à granja do usuário autenticado e definindo-o como ativo.
        /// </summary>
        /// <param name="modelAnimal">Dados do novo animal a ser cadastrado.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> PostAnimal([FromBody] CadastroAnimalDto modelAnimal)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Cria uma nova instância de Animal com os dados recebidos
            var novoAnimal = new Animal
            {
                Linhagem = modelAnimal.Linhagem,
                Idade = modelAnimal.Idade,
                Peso = modelAnimal.Peso,
                Status = null,
                UsuarioResponsavelId = usuarioId,
                Ativo = true,
                GranjaId = int.Parse(granjaId)
            };

            _context.Animais.Add(novoAnimal);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal cadastrado com sucesso!" });
        }

        /// <summary>
        /// Desativa um animal, alterando sua propriedade Ativo para false.
        /// O animal deve pertencer à granja do usuário autenticado.
        /// </summary>
        /// <param name="id">ID do animal a ser desativado.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaAnimais")]
        [HttpDelete("{id}/desativar")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            var granjaId = User.FindFirst("GranjaId")?.Value;
            if (granjaId == null)
            {
                return Unauthorized("Claims de granja não encontradas.");
            }

            // Busca o animal pelo ID e garante que pertence à granja do usuário autenticado
            var animal = await _context.Animais
                .Where(a => a.Id == id && a.GranjaId == int.Parse(granjaId))
                .FirstOrDefaultAsync();

            if (animal == null)
            {
                return NotFound("Animal não encontrado ou não pertence à sua granja.");
            }

            // Desativa o animal
            animal.Ativo = false;

            _context.Entry(animal).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal desativado com sucesso!" });
        }
    }
}
