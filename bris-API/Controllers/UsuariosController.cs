using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using bris_API.Data;
using bris_API.Models;
using bris_API.Services;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    /// <summary>
    /// Controller responsável pelas operações relacionadas aos usuários, como consulta, cadastro, edição, reativação e gerenciamento de vínculos.
    /// </summary>
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase, IUsuariosController
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        /// <summary>
        /// Construtor do controller que injeta as dependências necessárias.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        /// <param name="passwordService">Serviço para manipulação de senhas (hash, salt, etc.).</param>
        public UsuariosController(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Consulta e retorna a lista de usuários, aplicando filtros de visualização conforme a role do usuário autenticado.
        /// </summary>
        /// <returns>Lista de usuários mapeada para GetUsuarioDTO.</returns>
        [Authorize(Policy = "VisualizaUsuarios")]
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                IQueryable<Usuario> usuariosQuery = _context.Usuarios
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Role)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Agroindustria)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Granja)
                    .Where(u => !u.Vinculos.Any(v => v.RoleId == 99));

                if (Role == "GESTOR_AGRO")
                {
                    usuariosQuery = usuariosQuery
                        .Where(u => u.Vinculos.Any(v => v.AgroindustriaId == agroindustriaId));
                }
                else if (Role == "GESTOR_GRANJA")
                {
                    usuariosQuery = usuariosQuery
                        .Where(u => u.Vinculos.Any(v => v.GranjaId == granjaId));
                }

                var usuarios = await usuariosQuery
                    .Select(u => new GetUsuarioDTO
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        Email = u.Email,
                        CPF = u.CPF,
                        Telefone = u.Telefone,
                        DataCadastro = u.DataCadastro,
                        UltimoLogin = u.UltimoLogin,
                        Vinculos = u.Vinculos
                            .Where(v => Role == "ADMIN" ||
                                        (Role == "GESTOR_AGRO" && v.AgroindustriaId == agroindustriaId) ||
                                        (Role == "GESTOR_GRANJA" && v.GranjaId == granjaId))
                            .Select(v => new GetVinculoDTO
                            {
                                Id = v.Id,
                                Role = v.Role.Nome,
                                RoleId = v.RoleId,
                                NomeAgroindustria = v.Agroindustria != null ? v.Agroindustria.NomeFantasia : null,
                                AgroindustriaId = v.AgroindustriaId != null ? v.AgroindustriaId : null,
                                NomeGranja = v.Granja != null ? v.Granja.NomePropriedade : null,
                                GranjaId = v.GranjaId != null ? v.GranjaId : null
                            }).ToList()
                    })
                    .ToListAsync();

                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar usuários: " + ex.Message);
            }
        }

        /// <summary>
        /// Cadastra um novo usuário com múltiplos vínculos
        /// </summary>
        /// <param name="modelUsuario">Dados do usuário e seus vínculos</param>
        /// <returns>Mensagem de sucesso ou erro</returns>
        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CadastrarUsuarioDTO modelUsuario)
        {
            // Verifica duplicidade
            if (await _context.Usuarios.AnyAsync(u => u.Email == modelUsuario.Email))
                return BadRequest("Já existe um usuário com esse email cadastrado!");

            if (await _context.Usuarios.AnyAsync(u => u.CPF == modelUsuario.CPF))
                return BadRequest("Já existe um usuário com esse CPF cadastrado!");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try

            {
                Console.WriteLine("CPF recebido: " + modelUsuario.CPF);
                Console.WriteLine("Telefone recebido: " + modelUsuario.Telefone);
                // Cria usuário
                var usuario = new Usuario
                {
                    Nome = modelUsuario.Nome,
                    Email = modelUsuario.Email,
                    CPF = modelUsuario.CPF,
                    Telefone = modelUsuario.Telefone
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Cria senha
                var salt = _passwordService.GenerateSalt();
                var hash = _passwordService.HashPassword(modelUsuario.Senha, salt);

                _context.Senhas.Add(new Senha
                {
                    UsuarioId = usuario.Id,
                    SenhaHash = hash,
                    Salt = salt
                });
                await _context.SaveChangesAsync();

                // Cria vínculos
                foreach (var vinculoDto in modelUsuario.Vinculos)
                {
                    _context.Vinculos.Add(new Vinculo
                    {
                        UsuarioId = usuario.Id,
                        RoleId = vinculoDto.RoleId,
                        AgroindustriaId = vinculoDto.AgroindustriaId,
                        GranjaId = vinculoDto.GranjaId,
                        DataCriacao = DateTime.UtcNow
                    });
                }
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Usuário cadastrado com sucesso!",
                    usuarioId = usuario.Id
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro ao cadastrar usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Busca um usuário específico pelo ID, aplicando restrições de visualização baseadas na role do usuário autenticado.
        /// </summary>
        /// <param name="id">ID do usuário a ser buscado.</param>
        /// <returns>Dados do usuário no formato GetUsuarioDTO.</returns>
        [Authorize(Policy = "VisualizaUsuarios")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioPorId(int id)
        {
            try
            {
                var Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Obtém os IDs da Agroindústria e Granja do vínculo do usuário autenticado.
                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Carrega o usuário com seus vínculos e entidades relacionadas.
                var usuario = await _context.Usuarios
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Role)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Granja)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Agroindustria)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado!");
                }

                // Aplica restrições de visualização conforme a role do usuário autenticado.
                if (Role == "GESTOR_AGRO" && !usuario.Vinculos.Any(v => v.AgroindustriaId == agroindustriaId) ||
                    Role == "GESTOR_GRANJA" && !usuario.Vinculos.Any(v => v.GranjaId == granjaId))
                {
                    return Forbid("Você não tem permissão para visualizar este usuário.");
                }

                // Mapeia o usuário para o DTO, filtrando os vínculos conforme a role.
                var usuarioDTO = new GetUsuarioDTO
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    CPF = usuario.CPF,
                    Telefone = usuario.Telefone,
                    DataCadastro = usuario.DataCadastro,
                    UltimoLogin = usuario.UltimoLogin,
                    Vinculos = usuario.Vinculos
                .Where(v => Role == "ADMIN" ||
                            (Role == "GESTOR_AGRO" && v.AgroindustriaId == agroindustriaId) ||
                            (Role == "GESTOR_GRANJA" && v.GranjaId == granjaId))
                .Select(v => new GetVinculoDTO
                {
                    Id = v.Id,
                    Role = v.Role.Nome,
                    RoleId = v.RoleId,
                    NomeAgroindustria = v.Agroindustria?.NomeFantasia,
                    AgroindustriaId = v.AgroindustriaId,
                    NomeGranja = v.Granja?.NomePropriedade,
                    GranjaId = v.GranjaId
                }).ToList()
                };

                return Ok(usuarioDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar usuário: " + ex.Message);
            }
        }

        /// <summary>
        /// Edita os dados de um usuário existente, atualizando informações básicas, senha e vínculos, conforme permissões do usuário autenticado.
        /// </summary>
        /// <param name="id">ID do usuário a ser editado.</param>
        /// <param name="modelUsuario">Dados atualizados do usuário.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] EditarUsuarioDTO modelUsuario)
        {
            // Verifica duplicidade
            if (await _context.Usuarios.AnyAsync(u => u.Email == modelUsuario.Email && u.Id != id))
                return BadRequest("Já existe um usuário com esse email cadastrado!");

            if (await _context.Usuarios.AnyAsync(u => u.CPF == modelUsuario.CPF && u.Id != id))
                return BadRequest("Já existe um usuário com esse CPF cadastrado!");

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                var usuario = await _context.Usuarios
                    .Include(u => u.Vinculos)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null) return NotFound("Usuário não encontrado!");

                // Verificação de permissão
                if ((Role == "GESTOR_AGRO" && !usuario.Vinculos.Any(v => v.AgroindustriaId == agroindustriaId)) ||
                    (Role == "GESTOR_GRANJA" && !usuario.Vinculos.Any(v => v.GranjaId == granjaId)))
                {
                    return Forbid("Você não tem permissão para editar este usuário.");
                }

                // Atualização dos campos básicos
                usuario.Nome = modelUsuario.Nome;
                usuario.Email = modelUsuario.Email;
                usuario.CPF = modelUsuario.CPF;
                usuario.Telefone = modelUsuario.Telefone; // Novo campo adicionado

                // Atualização de senha
                if (!string.IsNullOrEmpty(modelUsuario.Senha))
                {
                    var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == id);
                    if (senha != null)
                    {
                        var salt = _passwordService.GenerateSalt();
                        senha.SenhaHash = _passwordService.HashPassword(modelUsuario.Senha, salt);
                        senha.Salt = salt;
                    }
                }

                // Gerenciamento de vínculos
                if (modelUsuario.Vinculos != null)
                {
                    // Remover vínculos existentes conforme permissões
                    var vinculosParaRemover = Role switch
                    {
                        "GESTOR_AGRO" => usuario.Vinculos
                            .Where(v => v.AgroindustriaId == agroindustriaId).ToList(),
                        "GESTOR_GRANJA" => usuario.Vinculos
                            .Where(v => v.GranjaId == granjaId).ToList(),
                        "ADMIN" => usuario.Vinculos.ToList(),
                        _ => new List<Vinculo>()
                    };

                    foreach (var vinculo in vinculosParaRemover)
                    {
                        usuario.Vinculos.Remove(vinculo);
                    }

                    // Adicionar novos vínculos com validação
                    foreach (var vinculoDto in modelUsuario.Vinculos)
                    {
                        // Validação de permissões
                        /* if (Role == "GESTOR_AGRO" &&
                            (vinculoDto.AgroindustriaId != agroindustriaId || vinculoDto.GranjaId != null))
                        {
                            return Forbid("Não pode criar vínculos fora da sua agroindústria");
                        }

                        if (Role == "GESTOR_GRANJA" &&
                            (vinculoDto.GranjaId != granjaId || vinculoDto.AgroindustriaId != null))
                        {
                            return Forbid("Não pode criar vínculos fora da sua granja");
                        } */

                        // Verificar se a role existe
                        if (!await _context.Roles.AnyAsync(r => r.Id == vinculoDto.RoleId))
                        {
                            return BadRequest($"Role ID {vinculoDto.RoleId} inválida");
                        }

                        usuario.Vinculos.Add(new Vinculo
                        {
                            RoleId = vinculoDto.RoleId,
                            AgroindustriaId = vinculoDto.AgroindustriaId,
                            GranjaId = vinculoDto.GranjaId,
                            UsuarioId = usuario.Id
                        });
                    }
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return Ok(new { message = "Usuário atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, "Erro ao editar usuário: " + ex.Message);
            }
        }

        /// <summary>
        /// Inativa um usuário, removendo todos os vínculos e criando um novo com role "INATIVO"
        /// </summary>
        [Authorize(Policy = "VisualizaTotal")]
        [HttpPut("inativar/{id}")]
        public async Task<IActionResult> InativarUsuario(int id)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Busca o usuário com todos os vínculos
                var usuario = await _context.Usuarios
                    .Include(u => u.Vinculos)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                    return NotFound("Usuário não encontrado");

                // Remove todos os vínculos existentes
                _context.Vinculos.RemoveRange(usuario.Vinculos);

                // Cria novo vínculo inativo
                var vinculoInativo = new Vinculo
                {
                    UsuarioId = usuario.Id,
                    RoleId = 99, // ID da role INATIVO
                    DataCriacao = DateTime.UtcNow
                };

                _context.Vinculos.Add(vinculoInativo);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro ao inativar usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Retorna a lista de usuários inativos (com vínculo com role "INATIVO").
        /// </summary>
        /// <returns>Lista de usuários inativos no formato GetUsuarioDTO.</returns>
        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet("inativos")]
        public async Task<IActionResult> GetUsuariosInativos()
        {
            try
            {
                var Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Consulta os usuários com vínculo cuja role é "INATIVO"
                IQueryable<Usuario> usuariosInativosQuery = _context.Usuarios
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Role)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Granja)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Agroindustria)
                    .Where(u => u.Vinculos.Any(v => v.Role.Nome == "INATIVO"));

                var usuariosInativos = await usuariosInativosQuery
                    .Select(u => new GetUsuarioDTO
                    {
                        Id = u.Id,
                        Nome = u.Nome,
                        Email = u.Email,
                        CPF = u.CPF,
                        Vinculos = u.Vinculos
                            .Where(v => v.Role.Nome == "INATIVO")
                            .Select(v => new GetVinculoDTO
                            {
                                Id = v.Id,
                                Role = v.Role.Nome,
                                RoleId = v.RoleId,
                                NomeAgroindustria = v.Agroindustria != null ? v.Agroindustria.NomeFantasia : null,
                                AgroindustriaId = v.AgroindustriaId != null ? v.AgroindustriaId : null,
                                NomeGranja = v.Granja != null ? v.Granja.NomePropriedade : null,
                                GranjaId = v.GranjaId != null ? v.GranjaId : null
                            }).ToList()
                    })
                    .ToListAsync();

                return Ok(usuariosInativos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar usuários inativos: " + ex.Message);
            }
        }

        /// <summary>
        /// Reativa um usuário inativo substituindo seu vínculo por novos vínculos
        /// </summary>
        [Authorize(Policy = "VisualizaTotal")]
        [HttpPut("reativar/{id}")]
        public async Task<IActionResult> ReativarUsuarioInativo(int id, [FromBody] List<SetVinculoDTO> novosVinculos)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // Pega os dados nas claims
                var role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoIdAutenticado = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                var vinculoAutenticado = await _context.Vinculos.FirstOrDefaultAsync(v => v.Id == vinculoIdAutenticado);

                // Validação básica
                if (!novosVinculos.Any())
                    return BadRequest("Deve ser fornecido pelo menos um vínculo");

                // Busca o usuário com vínculos inativos
                var usuario = await _context.Usuarios
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Role)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                    return NotFound("Usuário não encontrado");

                // Obtém todos os vínculos inativos
                var vinculosInativos = usuario.Vinculos
                    .Where(v => v.Role.Nome == "INATIVO")
                    .ToList();

                if (!vinculosInativos.Any())
                    return BadRequest("Usuário não está inativo");

                // Remove vínculos inativos
                _context.Vinculos.RemoveRange(vinculosInativos);

                // Validar e adicionar novos vínculos
                foreach (var setVinculoDto in novosVinculos)
                {
                    _context.Vinculos.Add(new Vinculo
                    {
                        UsuarioId = usuario.Id,
                        RoleId = setVinculoDto.RoleId,
                        AgroindustriaId = setVinculoDto.AgroindustriaId,
                        GranjaId = setVinculoDto.GranjaId,
                        DataCriacao = DateTime.UtcNow
                    });
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Erro ao reativar usuário: {ex.Message}");
            }
        }

        /// <summary>
        /// Retorna os vínculos de um usuário específico, aplicando filtros conforme a role do usuário autenticado.
        /// </summary>
        /// <param name="id">ID do usuário cujos vínculos serão retornados.</param>
        /// <returns>Lista de vínculos no formato GetVinculoDTO.</returns>
        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpGet("vinculos/{id}")]
        public async Task<IActionResult> GetVinculosPorUsuario(int id)
        {
            try
            {
                // Obtém informações do token do usuário autenticado
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Obtém os IDs de Agroindústria e Granja do vínculo do usuário autenticado
                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Busca os vínculos do usuário solicitado, incluindo as entidades relacionadas.
                var vinculosQuery = _context.Vinculos
                    .Where(v => v.UsuarioId == id)
                    .Include(v => v.Role)
                    .Include(v => v.Granja)
                    .Include(v => v.Agroindustria)
                    .AsQueryable();

                // Aplica filtro conforme a role do usuário autenticado
                if (userRole == "GESTOR_AGRO")
                {
                    vinculosQuery = vinculosQuery.Where(v => v.AgroindustriaId == agroindustriaId);
                }
                else if (userRole == "GESTOR_GRANJA")
                {
                    vinculosQuery = vinculosQuery.Where(v => v.GranjaId == granjaId);
                }

                var vinculos = await vinculosQuery
                    .Select(v => new GetVinculoDTO
                    {
                        Id = v.Id,
                        Role = v.Role.Nome,
                        RoleId = v.RoleId,
                        NomeAgroindustria = v.Agroindustria != null ? v.Agroindustria.NomeFantasia : null,
                        AgroindustriaId = v.AgroindustriaId != null ? v.AgroindustriaId : null,
                        NomeGranja = v.Granja != null ? v.Granja.NomePropriedade : null,
                        GranjaId = v.GranjaId != null ? v.GranjaId : null
                    })
                    .ToListAsync();

                return Ok(vinculos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar vínculos: " + ex.Message);
            }
        }
    }
}
