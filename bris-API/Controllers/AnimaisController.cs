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
        [HttpGet("ativos")]
        public async Task<ActionResult<IEnumerable<GetAnimalDTO>>> GetAnimaisAtivos()
        {
            var animais = await _context.Animais
                .Where(a => a.Ativo)
                .Select(a => new GetAnimalDTO
                {
                    Id = a.Id,
                    Linhagem = a.Linhagem,
                    Idade = a.Idade,
                    Peso = a.Peso,
                    Status = a.Status,
                    Ativo = a.Ativo,
                    DataCadastro = a.DataCadastro,
                    Granja = new GetGranjaDTO
                    {
                        Id = a.Granja!.Id,
                        NomePropriedade = a.Granja.NomePropriedade
                    },
                    UsuarioResponsavel = new GetUsuarioDTO
                    {
                        Id = a.Usuario!.Id,
                        Nome = a.Usuario.Nome
                    }
                }).ToListAsync();

            return Ok(animais);
        }


        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("ativos/granja")]
        public async Task<ActionResult<IEnumerable<GetAnimalDTO>>> GetAnimaisAtivosPorGranja()
        {
            var granjaIdClaim = User.FindFirst("GranjaId")?.Value;
            if (granjaIdClaim == null)
                return Unauthorized("Claim de granja não encontrada.");

            var granjaId = int.Parse(granjaIdClaim);

            var animais = await _context.Animais
                .Where(a => a.GranjaId == granjaId && a.Ativo)
                .Select(a => new GetAnimalDTO
                {
                    Id = a.Id,
                    Linhagem = a.Linhagem,
                    Idade = a.Idade,
                    Peso = a.Peso,
                    Status = a.Status,
                    Ativo = a.Ativo,
                    DataCadastro = a.DataCadastro,
                    UsuarioResponsavel = new GetUsuarioDTO
                    {
                        Id = a.Usuario!.Id,
                        Nome = a.Usuario.Nome
                    }
                }).ToListAsync();

            return Ok(animais);
        }


        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("ativos/agroindustria")]
        public async Task<ActionResult<IEnumerable<GetAnimalDTO>>> GetAnimaisAtivosPorAgroindustria()
        {
            var agroIdClaim = User.FindFirst("AgroindustriaId")?.Value;
            if (agroIdClaim == null)
                return Unauthorized("Claim de agroindústria não encontrada.");

            var agroId = int.Parse(agroIdClaim);

            var animais = await _context.Animais
                .Where(a => a.Granja!.AgroindustriaId == agroId && a.Ativo)
                .Select(a => new GetAnimalDTO
                {
                    Id = a.Id,
                    Linhagem = a.Linhagem,
                    Idade = a.Idade,
                    Peso = a.Peso,
                    Status = a.Status,
                    Ativo = a.Ativo,
                    DataCadastro = a.DataCadastro,
                    Granja = new GetGranjaDTO
                    {
                        Id = a.Granja.Id,
                        NomePropriedade = a.Granja.NomePropriedade
                    },
                    UsuarioResponsavel = new GetUsuarioDTO
                    {
                        Id = a.Usuario!.Id,
                        Nome = a.Usuario.Nome
                    }
                }).ToListAsync();

            return Ok(animais);
        }

        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("inativos")]
        public async Task<ActionResult<IEnumerable<GetAnimalDTO>>> GetAnimaisInativos()
        {
            var animais = await _context.Animais
                .Where(a => !a.Ativo)
                .Select(a => new GetAnimalDTO
                {
                    Id = a.Id,
                    Linhagem = a.Linhagem,
                    Idade = a.Idade,
                    Peso = a.Peso,
                    Status = a.Status,
                    Ativo = a.Ativo,
                    DataCadastro = a.DataCadastro,
                    Granja = a.Granja != null ? new GetGranjaDTO
                    {
                        Id = a.Granja.Id,
                        NomePropriedade = a.Granja.NomePropriedade
                    } : null,
                    UsuarioResponsavel = a.Usuario != null ? new GetUsuarioDTO
                    {
                        Id = a.Usuario.Id,
                        Nome = a.Usuario.Nome
                    } : null
                })
                .ToListAsync();

            return Ok(animais);
        }

        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("inativos/granja")]
        public async Task<ActionResult<IEnumerable<GetAnimalDTO>>> GetAnimaisInativosPorGranja()
        {
            var granjaIdClaim = User.FindFirst("GranjaId")?.Value;
            if (granjaIdClaim == null)
                return Unauthorized("Claim de granja não encontrada.");

            var granjaId = int.Parse(granjaIdClaim);

            var animais = await _context.Animais
                .Where(a => a.GranjaId == granjaId && !a.Ativo)
                .Select(a => new GetAnimalDTO
                {
                    Id = a.Id,
                    Linhagem = a.Linhagem,
                    Idade = a.Idade,
                    Peso = a.Peso,
                    Status = a.Status,
                    Ativo = a.Ativo,
                    DataCadastro = a.DataCadastro,
                    UsuarioResponsavel = a.Usuario != null ? new GetUsuarioDTO
                    {
                        Id = a.Usuario.Id,
                        Nome = a.Usuario.Nome
                    } : null
                })
                .ToListAsync();

            return Ok(animais);
        }

        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("inativos/agroindustria")]
        public async Task<ActionResult<IEnumerable<GetAnimalDTO>>> GetAnimaisInativosPorAgroindustria()
        {
            var agroIdClaim = User.FindFirst("AgroindustriaId")?.Value;
            if (agroIdClaim == null)
                return Unauthorized("Claim de agroindústria não encontrada.");

            var agroId = int.Parse(agroIdClaim);

            var animais = await _context.Animais
                .Where(a => a.Granja != null && a.Granja.AgroindustriaId == agroId && !a.Ativo)
                .Select(a => new GetAnimalDTO
                {
                    Id = a.Id,
                    Linhagem = a.Linhagem,
                    Idade = a.Idade,
                    Peso = a.Peso,
                    Status = a.Status,
                    Ativo = a.Ativo,
                    DataCadastro = a.DataCadastro,
                    Granja = new GetGranjaDTO
                    {
                        Id = a.Granja.Id,
                        NomePropriedade = a.Granja.NomePropriedade
                    },
                    UsuarioResponsavel = a.Usuario != null ? new GetUsuarioDTO
                    {
                        Id = a.Usuario.Id,
                        Nome = a.Usuario.Nome
                    } : null
                })
                .ToListAsync();

            return Ok(animais);
        }

        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("ativar/{id}")]
        public async Task<IActionResult> AtivarAnimal(int id)
        {
            var animal = await _context.Animais.Include(a => a.Granja)
                                            .ThenInclude(g => g.Agroindustria)
                                            .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
                return NotFound("Animal não encontrado.");

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var usuarioGranjaId = int.Parse(User.FindFirst("GranjaId")?.Value ?? "0");
            var usuarioAgroId = int.Parse(User.FindFirst("AgroindustriaId")?.Value ?? "0");

            // Validação de acesso
            if (role != "ADMIN")
            {
                if (role == "GESTOR_AGRO" && animal.Granja?.AgroindustriaId != usuarioAgroId)
                    return Forbid("Você não tem permissão para ativar este animal.");

                if ((role == "GESTOR_GRANJA" || role == "TECNICO") && animal.GranjaId != usuarioGranjaId)
                    return Forbid("Você não tem permissão para ativar este animal.");
            }

            animal.Ativo = true;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal ativado com sucesso!" });
        }

        [Authorize(Policy = "VisualizaAnimais")]
        [HttpGet("{id}")]
        public async Task<ActionResult<GetAnimalDTO>> GetAnimal(int id)
        {
            var animal = await _context.Animais
                .Include(a => a.Granja)
                    .ThenInclude(g => g.Agroindustria)
                .Include(a => a.Usuario)
                .Include(a => a.Avaliacoes)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
                return NotFound("Animal não encontrado.");

            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            var usuarioGranjaId = int.Parse(User.FindFirst("GranjaId")?.Value ?? "0");
            var usuarioAgroId = int.Parse(User.FindFirst("AgroindustriaId")?.Value ?? "0");

            // Validação de acesso
            if (role != "ADMIN")
            {
                if (role == "GESTOR_AGRO" && animal.Granja?.AgroindustriaId != usuarioAgroId)
                    return Forbid("Você não tem permissão para visualizar este animal.");

                if ((role == "GESTOR_GRANJA" || role == "TECNICO") && animal.GranjaId != usuarioGranjaId)
                    return Forbid("Você não tem permissão para visualizar este animal.");
            }

            // Mapeando para DTO
            var result = new GetAnimalDTO
            {
                Id = animal.Id,
                Linhagem = animal.Linhagem,
                Idade = animal.Idade,
                Peso = animal.Peso,
                Status = animal.Status,
                Ativo = animal.Ativo,
                DataCadastro = animal.DataCadastro,
                Granja = new GetGranjaDTO
                {
                    Id = animal.Granja?.Id ?? 0,
                    NomePropriedade = animal.Granja?.NomePropriedade,
                    CNPJ = animal.Granja?.CNPJ,
                    Telefone = animal.Granja?.Telefone,
                    Email = animal.Granja?.Email,
                    Ativo = animal.Granja?.Ativo ?? false,
                    Agroindustria = new GetAgroindustriaDTO
                    {
                        Id = animal.Granja?.Agroindustria?.Id ?? 0,
                        NomeFantasia = animal.Granja?.Agroindustria?.NomeFantasia,
                        RazaoSocial = animal.Granja?.Agroindustria?.RazaoSocial,
                        CNPJ = animal.Granja?.Agroindustria?.CNPJ,
                        Email = animal.Granja?.Agroindustria?.Email,
                        Telefone = animal.Granja?.Agroindustria?.Telefone,
                        Ativo = animal.Granja?.Agroindustria?.Ativo ?? false
                    }
                },
                UsuarioResponsavel = new GetUsuarioDTO
                {
                    Id = animal.Usuario?.Id ?? 0,
                    Nome = animal.Usuario?.Nome,
                    Email = animal.Usuario?.Email
                },
                Avaliacoes = animal.Avaliacoes?.Select(av => new GetAvaliacaoDTO
                {
                    //preencher os campos necessários da avaliação depois
                }).ToList()
            };

            return Ok(result);
        }

        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> PutAnimal(int id, [FromBody] AnimalDto modelAnimal)
        {
            // Validação inicial do DTO
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var animal = await _context.Animais
                .Include(a => a.Granja)
                .Include(a => a.Usuario)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (animal == null)
                return NotFound($"Animal com ID {id} não encontrado.");

            // Validação de acesso baseada na role
            var role = User.FindFirst(c => c.Type == ClaimTypes.Role)?.Value;
            var usuarioGranjaId = int.Parse(User.FindFirst("GranjaId")?.Value ?? "0");
            var usuarioAgroindustriaId = int.Parse(User.FindFirst("AgroindustriaId")?.Value ?? "0");

            if (role != "ADMIN")
            {
                if (role == "GESTOR_AGRO" && animal.Granja?.AgroindustriaId != usuarioAgroindustriaId)
                    return Forbid("Animal não pertence à sua agroindústria.");
                if ((role == "GESTOR_GRANJA" || role == "TECNICO") && animal.GranjaId != usuarioGranjaId)
                    return Forbid("Animal não pertence à sua granja.");
            }

            // Atualiza os campos do animal
            animal.Linhagem = modelAnimal.Linhagem;
            animal.Idade = modelAnimal.Idade;
            animal.Peso = modelAnimal.Peso;
            animal.Status = modelAnimal.Status;
            animal.GranjaId = modelAnimal.GranjaId;
            animal.UsuarioResponsavelId = modelAnimal.UsuarioResponsavelId;
            animal.Ativo = modelAnimal.Ativo;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal atualizado com sucesso!" });
        }


        [Authorize(Policy = "GerenciaAnimais")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> PostAnimal([FromBody] AnimalDto modelAnimal)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Pega informações do usuário autenticado
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var usuarioGranjaId = int.Parse(User.FindFirst("GranjaId")?.Value ?? "0");
            var usuarioAgroId = int.Parse(User.FindFirst("AgroindustriaId")?.Value ?? "0");

            // Valida permissões de cadastro de acordo com a role
            if (userRole == "GESTOR_GRANJA" || userRole == "TECNICO")
            {
                if (modelAnimal.GranjaId != usuarioGranjaId)
                    return Forbid("Você só pode cadastrar animais na sua própria granja.");
            }
            else if (userRole == "GESTOR_AGRO")
            {
                var granja = await _context.Granjas.FindAsync(modelAnimal.GranjaId);
                if (granja == null || granja.AgroindustriaId != usuarioAgroId)
                    return Forbid("Você só pode cadastrar animais em granjas da sua agroindústria.");
            }
            // ADMIN não precisa de validação adicional

            // Cria o animal
            var animal = new Animal
            {
                Linhagem = modelAnimal.Linhagem,
                Idade = modelAnimal.Idade,
                Peso = modelAnimal.Peso,
                Status = modelAnimal.Status,
                GranjaId = modelAnimal.GranjaId,
                UsuarioResponsavelId = modelAnimal.UsuarioResponsavelId,
                Ativo = modelAnimal.Ativo,
                DataCadastro = DateTime.UtcNow
            };

            _context.Animais.Add(animal);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal cadastrado com sucesso!" });
        }


        [Authorize(Policy = "GerenciaAnimais")]
        [HttpDelete("{id}/desativar")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            // Busca o animal
            var animal = await _context.Animais.FindAsync(id);
            if (animal == null)
                return NotFound("Animal não encontrado.");

            // Pega informações do usuário autenticado
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var usuarioGranjaId = int.Parse(User.FindFirst("GranjaId")?.Value ?? "0");
            var usuarioAgroId = int.Parse(User.FindFirst("AgroindustriaId")?.Value ?? "0");

            // Valida permissões de acordo com a role
            if (userRole == "GESTOR_GRANJA" || userRole == "TECNICO")
            {
                if (animal.GranjaId != usuarioGranjaId)
                    return Forbid("Você só pode desativar animais da sua própria granja.");
            }
            else if (userRole == "GESTOR_AGRO")
            {
                var granja = await _context.Granjas.FindAsync(animal.GranjaId);
                if (granja == null || granja.AgroindustriaId != usuarioAgroId)
                    return Forbid("Você só pode desativar animais em granjas da sua agroindústria.");
            }
            // ADMIN pode desativar qualquer animal

            // Desativa o animal
            animal.Ativo = false;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Animal desativado com sucesso!" });
        }
    }
}
