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
                // Recupera a Role e o ID do vínculo a partir do token do usuário autenticado.
                var Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Inicializa a consulta de usuários, incluindo os vínculos e suas entidades relacionadas.
                IQueryable<Usuario> usuariosQuery = _context.Usuarios
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Role)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Agroindustria)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Granja);

                // Obtém os IDs da Agroindústria e Granja do vínculo do usuário autenticado.
                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Aplica filtro conforme a Role do usuário:
                // - GESTOR_AGRO: apenas usuários com vínculos na mesma Agroindústria.
                // - GESTOR_GRANJA: apenas usuários com vínculos na mesma Granja.
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

                // Mapeia os usuários para o DTO (GetUsuarioDTO), aplicando filtro nos vínculos conforme a Role.
                var usuarios = await usuariosQuery
                    .Select(u => new GetUsuarioDTO
                    {
                        Nome = u.Nome,
                        Email = u.Email,
                        CPF = u.CPF,
                        Vinculos = u.Vinculos
                            .Where(v => Role == "ADMIN" || 
                                        (Role == "GESTOR_AGRO" && v.AgroindustriaId == agroindustriaId) ||
                                        (Role == "GESTOR_GRANJA" && v.GranjaId == granjaId))
                            .Select(v => new GetVinculoDTO
                            {
                                Id = v.Id,
                                Role = v.Role.Nome,
                                NomeAgroindustria = v.Agroindustria.NomeFantasia,
                                NomeGranja = v.Granja.NomePropriedade
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
        /// Cadastra um novo usuário, criando também sua senha e vínculo.
        /// </summary>
        /// <param name="modelUsuario">Dados do usuário a ser cadastrado.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CadastrarUsuarioDTO modelUsuario)
        {
            // Verifica se já existe um usuário com o mesmo email ou CPF
            if (await _context.Usuarios.AnyAsync(u => u.Email == modelUsuario.Email || u.CPF == modelUsuario.CPF))
                return BadRequest("Já existe um usuário com esse email ou CPF cadastrado!");

            try
            {
                // Cria um novo usuário com os dados fornecidos
                var usuario = new Usuario
                {
                    Nome = modelUsuario.Nome,
                    Email = modelUsuario.Email,
                    CPF = modelUsuario.CPF
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                // Gera o salt e o hash da senha "123456" ou a senha informada
                var salt = _passwordService.GenerateSalt();
                var hash = _passwordService.HashPassword(modelUsuario.Senha, salt);

                // Cria a entidade Senha relacionada ao usuário
                var senha = new Senha
                {
                    UsuarioId = usuario.Id,
                    SenhaHash = hash,
                    Salt = salt
                };

                _context.Senhas.Add(senha);
                await _context.SaveChangesAsync();

                // Cria o vínculo do usuário, associando a role, granja e agroindústria conforme o DTO
                var vinculo = new Vinculo
                {
                    UsuarioId = usuario.Id,
                    RoleId = modelUsuario.RoleId,
                    GranjaId = modelUsuario.GranjaId,
                    AgroindustriaId = modelUsuario.AgroindustriaId
                };

                _context.Vinculos.Add(vinculo);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Usuário registrado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao cadastrar o usuário: " + ex.Message);
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
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    CPF = usuario.CPF,
                    Vinculos = usuario.Vinculos
                        .Where(v => Role == "ADMIN" ||
                                    (Role == "GESTOR_AGRO" && v.AgroindustriaId == agroindustriaId) ||
                                    (Role == "GESTOR_GRANJA" && v.GranjaId == granjaId))
                        .Select(v => new GetVinculoDTO
                        {
                            Id = v.Id,
                            Role = v.Role.Nome,
                            NomeGranja = v.Granja?.NomePropriedade,
                            NomeAgroindustria = v.Agroindustria?.NomeFantasia
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
            try
            {
                var Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Obtém os IDs da Agroindústria e Granja do vínculo do usuário autenticado.
                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Verifica se o usuário a ser editado existe e carrega seus vínculos.
                var usuario = await _context.Usuarios
                    .Include(u => u.Vinculos)
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado!");
                }

                // Aplica restrições de edição conforme a role do usuário autenticado.
                if (Role == "GESTOR_AGRO" && !usuario.Vinculos.Any(v => v.AgroindustriaId == agroindustriaId) ||
                    Role == "GESTOR_GRANJA" && !usuario.Vinculos.Any(v => v.GranjaId == granjaId))
                {
                    return Forbid("Você não tem permissão para editar este usuário.");
                }

                // Atualiza os campos do usuário
                usuario.Nome = modelUsuario.Nome;
                usuario.Email = modelUsuario.Email;
                usuario.CPF = modelUsuario.CPF;

                // Atualiza a senha se um novo valor for fornecido
                if (modelUsuario.Senha != null)
                {
                    var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == id);
                    if (senha != null)
                    {
                        var salt = _passwordService.GenerateSalt();
                        senha.SenhaHash = _passwordService.HashPassword(modelUsuario.Senha, salt);
                        senha.Salt = salt;
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Usuário atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao editar usuário: " + ex.Message);
            }
        }

        /// <summary>
        /// Retorna a lista de usuários inativos (com vínculo com role "INATIVO"), aplicando filtros conforme a role do usuário autenticado.
        /// </summary>
        /// <returns>Lista de usuários inativos no formato GetUsuarioDTO.</returns>
        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpGet("reativar")]
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

                // Aplica filtro conforme a role do usuário autenticado
                if (Role == "GESTOR_AGRO")
                {
                    usuariosInativosQuery = usuariosInativosQuery
                        .Where(u => u.Vinculos.Any(v => v.AgroindustriaId == agroindustriaId));
                }
                else if (Role == "GESTOR_GRANJA")
                {
                    usuariosInativosQuery = usuariosInativosQuery
                        .Where(u => u.Vinculos.Any(v => v.GranjaId == granjaId));
                }

                var usuariosInativos = await usuariosInativosQuery
                    .Select(u => new GetUsuarioDTO
                    {
                        Nome = u.Nome,
                        Email = u.Email,
                        CPF = u.CPF,
                        Vinculos = u.Vinculos
                            .Where(v => v.Role.Nome == "INATIVO" &&
                                    (Role == "ADMIN" ||
                                        (Role == "GESTOR_AGRO" && v.AgroindustriaId == agroindustriaId) ||
                                        (Role == "GESTOR_GRANJA" && v.GranjaId == granjaId)))
                            .Select(v => new GetVinculoDTO
                            {
                                Id = v.Id,
                                Role = v.Role.Nome,
                                NomeAgroindustria = v.Agroindustria != null ? v.Agroindustria.NomeFantasia : null,
                                NomeGranja = v.Granja != null ? v.Granja.NomePropriedade : null
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
        /// Reativa um usuário inativo alterando seu vínculo, caso o usuário autenticado possua permissão.
        /// </summary>
        /// <param name="id">ID do usuário a ser reativado.</param>
        /// <param name="modelAtivar">Dados para atualização do vínculo.</param>
        /// <returns>O vínculo atualizado no formato GetVinculoDTO.</returns>
        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPut("reativar/{id}")]
        public async Task<IActionResult> ReativarUsuarioInativo(int id, [FromBody] AtivarUsuarioDto modelAtivar)
        {
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

                // Busca o primeiro vínculo com role "INATIVO" para o usuário especificado
                var vinculo = await _context.Vinculos
                    .Include(v => v.Role)
                    .FirstOrDefaultAsync(v => v.UsuarioId == id && v.Role.Nome == "INATIVO");

                if (vinculo == null)
                {
                    return NotFound("Usuário não encontrado!");
                }

                // Verifica se o usuário autenticado tem permissão para reativar o vínculo
                if ((Role == "GESTOR_AGRO" && vinculo.AgroindustriaId != agroindustriaId) ||
                    (Role == "GESTOR_GRANJA" && vinculo.GranjaId != granjaId))
                {
                    return Forbid("Você não tem permissão para ativar este usuário.");
                }

                // Atualiza o vínculo conforme os dados do DTO
                vinculo.RoleId = modelAtivar.RoleId;
                vinculo.GranjaId = modelAtivar.GranjaId;
                vinculo.AgroindustriaId = modelAtivar.AgroindustriaId;

                await _context.SaveChangesAsync();

                return Ok(new GetVinculoDTO
                {
                    Id = vinculo.Id,
                    Role = vinculo.Role.Nome,
                    NomeGranja = vinculo.Granja?.NomePropriedade,
                    NomeAgroindustria = vinculo.Agroindustria?.NomeFantasia
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao reativar usuário: " + ex.Message);
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
                        NomeAgroindustria = v.Agroindustria != null ? v.Agroindustria.NomeFantasia : null,
                        NomeGranja = v.Granja != null ? v.Granja.NomePropriedade : null
                    })
                    .ToListAsync();

                return Ok(vinculos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar vínculos: " + ex.Message);
            }
        }

        /// <summary>
        /// Edita um vínculo existente, permitindo alterações conforme as permissões do usuário autenticado.
        /// </summary>
        /// <param name="vinculoId">ID do vínculo a ser editado.</param>
        /// <param name="modelVinculo">Dados para atualizar o vínculo.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPut("vinculos/editar/{vinculoId}")]
        public async Task<IActionResult> EditarVinculo(int vinculoId, [FromBody] SetVinculoDTO modelVinculo)
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var usuarioVinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Obtém os IDs de Agroindústria e Granja do vínculo do usuário autenticado
                var usuarioAgroindustriaId = _context.Vinculos
                    .Where(v => v.Id == usuarioVinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var usuarioGranjaId = _context.Vinculos
                    .Where(v => v.Id == usuarioVinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Busca o vínculo a ser editado, incluindo suas entidades relacionadas
                var vinculo = await _context.Vinculos
                    .Include(v => v.Role)
                    .Include(v => v.Granja)
                    .Include(v => v.Agroindustria)
                    .FirstOrDefaultAsync(v => v.Id == vinculoId);

                if (vinculo == null)
                {
                    return NotFound("Vínculo não encontrado!");
                }

                // Aplica regras de edição com base na role do usuário autenticado
                if (userRole == "GESTOR_AGRO")
                {
                    // GESTOR_AGRO pode editar apenas se o vínculo pertencer à mesma Agroindústria.
                    if (vinculo.AgroindustriaId != usuarioAgroindustriaId)
                    {
                        return Forbid("Você só pode editar vínculos da sua própria Agroindústria.");
                    }
                    vinculo.RoleId = modelVinculo.RoleId;
                    vinculo.GranjaId = modelVinculo.GranjaId ?? vinculo.GranjaId; // Mantém valor atual se nulo
                }
                else if (userRole == "GESTOR_GRANJA")
                {
                    // GESTOR_GRANJA pode editar apenas se o vínculo pertencer à mesma Granja.
                    if (vinculo.GranjaId != usuarioGranjaId)
                    {
                        return Forbid("Você só pode editar vínculos da sua própria Granja.");
                    }
                    vinculo.RoleId = modelVinculo.RoleId;
                }
                else if (userRole == "ADMIN")
                {
                    // ADMIN pode editar todos os campos
                    vinculo.RoleId = modelVinculo.RoleId;
                    vinculo.GranjaId = modelVinculo.GranjaId ?? vinculo.GranjaId;
                    vinculo.AgroindustriaId = modelVinculo.AgroindustriaId ?? vinculo.AgroindustriaId;
                }
                else
                {
                    return Forbid("Permissão insuficiente para editar vínculos.");
                }

                await _context.SaveChangesAsync();

                return Ok(new { message = "Vínculo atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao editar vínculo: " + ex.Message);
            }
        }

        /// <summary>
        /// Adiciona um novo vínculo para um usuário específico, configurando os campos de acordo com a role do usuário autenticado.
        /// </summary>
        /// <param name="id">ID do usuário para o qual será adicionado o vínculo.</param>
        /// <param name="modelVinculo">Dados do novo vínculo.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPost("novo-vinculo/{id}")]
        public async Task<IActionResult> AdicionarVinculoPorUsuario(int id, [FromBody] SetVinculoDTO modelVinculo)
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var usuarioVinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Obtém os IDs de Agroindústria e Granja do vínculo do usuário autenticado
                var usuarioAgroindustriaId = _context.Vinculos
                    .Where(v => v.Id == usuarioVinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var usuarioGranjaId = _context.Vinculos
                    .Where(v => v.Id == usuarioVinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Cria um novo vínculo para o usuário com base na role do usuário autenticado
                var novoVinculo = new Vinculo
                {
                    UsuarioId = id,
                    RoleId = modelVinculo.RoleId
                };

                if (userRole == "GESTOR_AGRO")
                {
                    // GESTOR_AGRO utiliza a Agroindústria do próprio vínculo e permite definir o GranjaId via DTO.
                    novoVinculo.GranjaId = modelVinculo.GranjaId;
                    novoVinculo.AgroindustriaId = usuarioAgroindustriaId;
                }
                else if (userRole == "GESTOR_GRANJA")
                {
                    // GESTOR_GRANJA utiliza os IDs de Granja e Agroindústria do próprio vínculo.
                    novoVinculo.GranjaId = usuarioGranjaId;
                    novoVinculo.AgroindustriaId = usuarioAgroindustriaId;
                }
                else if (userRole == "ADMIN")
                {
                    // ADMIN pode definir todos os campos conforme o DTO.
                    novoVinculo.GranjaId = modelVinculo.GranjaId;
                    novoVinculo.AgroindustriaId = modelVinculo.AgroindustriaId;
                }
                else
                {
                    return Forbid("Permissão insuficiente para adicionar vínculo.");
                }

                await _context.Vinculos.AddAsync(novoVinculo);
                await _context.SaveChangesAsync();

                return Ok(new { message = "Vínculo adicionado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao adicionar vínculo: " + ex.Message);
            }
        }
    }
}
