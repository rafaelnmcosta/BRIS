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
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase, IUsuariosController
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public UsuariosController(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        [Authorize(Policy = "VisualizaUsuarios")]
        [HttpGet]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Inicializa a consulta de usuários incluindo os vínculos e aplicando o filtro baseado na Role
                IQueryable<Usuario> usuariosQuery = _context.Usuarios
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Role) // Incluindo Role para acessar os dados
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Agroindustria) // Incluindo Agroindústria para acesso aos dados
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Granja); // Incluindo Granja para acesso aos dados

                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                if (Role == "GESTOR_AGRO")
                {
                    // GESTOR_AGRO: Filtra usuários pela Agroindústria do vínculo
                    usuariosQuery = usuariosQuery
                        .Where(u => u.Vinculos.Any(v => v.AgroindustriaId == agroindustriaId));
                }
                else if (Role == "GESTOR_GRANJA")
                {
                    // GESTOR_GRANJA: Filtra usuários pela Granja do vínculo
                    usuariosQuery = usuariosQuery
                        .Where(u => u.Vinculos.Any(v => v.GranjaId == granjaId));
                }

                // Executar a consulta e mapear para GetUsuarioDTO com filtro nos vínculos
                var usuarios = await usuariosQuery
                    .Select(u => new GetUsuarioDTO
                    {
                        Nome = u.Nome,
                        Email = u.Email,
                        CPF = u.CPF,
                        Vinculos = u.Vinculos
                            .Where(v => Role == "ADMIN" || // Pra admin não tem filtro e exibe todos os vínculos
                                        (Role == "GESTOR_AGRO" && v.AgroindustriaId == agroindustriaId) || // Filtra os vínculos para exibir apenas os que são da mesma agroindustria
                                        (Role == "GESTOR_GRANJA" && v.GranjaId == granjaId)) // Apenas os vinculos na mesma granja
                            .Select(v => new GetVinculoDTO
                            {
                                VinculoId = v.Id,
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

        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CadastrarUsuarioDTO modelUsuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == modelUsuario.Email || u.CPF == modelUsuario.CPF))
                return BadRequest("Já existe um usuário com esse email ou CPF cadastrado!");

            try
            {
                var usuario = new Usuario
                {
                    Nome = modelUsuario.Nome,
                    Email = modelUsuario.Email,
                    CPF = modelUsuario.CPF
                };

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();

                var salt = _passwordService.GenerateSalt();
                var hash = _passwordService.HashPassword(modelUsuario.Senha, salt);

                var senha = new Senha
                {
                    UsuarioId = usuario.Id,
                    SenhaHash = hash,
                    Salt = salt
                };

                _context.Senhas.Add(senha);
                await _context.SaveChangesAsync();

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

        [Authorize(Policy = "VisualizaUsuarios")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuarioPorId(int id)
        {
            try
            {
                var Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Obtém AgroindustriaId e GranjaId do vínculo do usuário que está fazendo a requisição
                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Carrega o usuário específico, incluindo os vínculos
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

                // Aplica as restrições de visualização com base no Role do usuário que faz a requisição
                if (Role == "GESTOR_AGRO" && !usuario.Vinculos.Any(v => v.AgroindustriaId == agroindustriaId) ||
                    Role == "GESTOR_GRANJA" && !usuario.Vinculos.Any(v => v.GranjaId == granjaId))
                {
                    return Forbid("Você não tem permissão para visualizar este usuário.");
                }

                // Mapeia para GetUsuarioDTO aplicando filtro nos vínculos
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
                            VinculoId = v.Id,
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

        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPut("editar/{id}")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] EditarUsuarioDTO modelUsuario)
        {
            try
            {
                var Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Obter agroindustriaId e granjaId do usuário autenticado
                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Verificar se o usuário a ser editado existe
                var usuario = await _context.Usuarios
                    .Include(u => u.Vinculos) // Incluir os vínculos do usuário
                    .FirstOrDefaultAsync(u => u.Id == id);

                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado!");
                }

                // Aplicar restrições de edição com base no Role do usuário autenticado
                if (Role == "GESTOR_AGRO" && !usuario.Vinculos.Any(v => v.AgroindustriaId == agroindustriaId) ||
                    Role == "GESTOR_GRANJA" && !usuario.Vinculos.Any(v => v.GranjaId == granjaId))
                {
                    return Forbid("Você não tem permissão para editar este usuário.");
                }

                // Atualizar os campos do usuário
                usuario.Nome = modelUsuario.Nome;
                usuario.Email = modelUsuario.Email;
                usuario.CPF = modelUsuario.CPF;

                // Atualizar a senha se ela vier diferente de null
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

        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpGet("ativar")]
        public async Task<IActionResult> GetUsuariosPendentes()
        {
            try
            {
                var Role = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Obtém AgroindustriaId e GranjaId do vínculo do usuário que está fazendo a requisição
                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Consulta os usuários não ativados (com vínculo cuja Role é "PENDENTE")
                IQueryable<Usuario> usuariosPendentesQuery = _context.Usuarios
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Role)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Granja)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Agroindustria)
                    .Where(u => u.Vinculos.Any(v => v.Role.Nome == "PENDENTE"));

                // Aplica filtros de acordo com a Role do usuário
                if (Role == "GESTOR_AGRO")
                {
                    usuariosPendentesQuery = usuariosPendentesQuery
                        .Where(u => u.Vinculos.Any(v => v.AgroindustriaId == agroindustriaId));
                }
                else if (Role == "GESTOR_GRANJA")
                {
                    usuariosPendentesQuery = usuariosPendentesQuery
                        .Where(u => u.Vinculos.Any(v => v.GranjaId == granjaId));
                }

                // Executa a consulta e transforma em GetUsuarioDTO com o filtro de vínculos aplicados
                var usuariosPendentes = await usuariosPendentesQuery
                .Select(u => new GetUsuarioDTO
                {
                    Nome = u.Nome,
                    Email = u.Email,
                    CPF = u.CPF,
                    Vinculos = u.Vinculos
                        .Where(v => v.Role.Nome == "PENDENTE" &&
                                (Role == "ADMIN" ||
                                    (Role == "GESTOR_AGRO" && v.AgroindustriaId == agroindustriaId) ||
                                    (Role == "GESTOR_GRANJA" && v.GranjaId == granjaId)))
                        .Select(v => new GetVinculoDTO
                        {
                            VinculoId = v.Id,
                            Role = v.Role.Nome,
                            NomeAgroindustria = v.Agroindustria != null ? v.Agroindustria.NomeFantasia : null,
                            NomeGranja = v.Granja != null ? v.Granja.NomePropriedade : null
                        }).ToList()
                })
                .ToListAsync();

                return Ok(usuariosPendentes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar usuários pendentes: " + ex.Message);
            }
        }

        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPost("ativar/{id}")]
        public async Task<IActionResult> AtivarUsuarioPendente(int id, [FromBody] AtivarUsuarioDto modelAtivar)
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

                // Busca o primeiro vínculo correspondente na tabela Vinculos com a Role "PENDENTE"
                var vinculo = await _context.Vinculos
                    .Include(v => v.Role)
                    .FirstOrDefaultAsync(v => v.UsuarioId == id && v.Role.Nome == "PENDENTE");

                if (vinculo == null)
                {
                    return NotFound("Usuário não encontrado!");
                }

                // Verificações de permissão com base no Role do usuário autenticado
                if ((Role == "GESTOR_AGRO" && vinculo.AgroindustriaId != agroindustriaId) ||
                    (Role == "GESTOR_GRANJA" && vinculo.GranjaId != granjaId))
                {
                    return Forbid("Você não tem permissão para ativar este usuário.");
                }

                // Atualizar o vínculo com os novos valores do DTO
                vinculo.RoleId = modelAtivar.RoleId;
                vinculo.GranjaId = modelAtivar.GranjaId;
                vinculo.AgroindustriaId = modelAtivar.AgroindustriaId;

                await _context.SaveChangesAsync();

                // Retorna o vínculo atualizado
                return Ok(new GetVinculoDTO
                {
                    VinculoId = vinculo.Id,
                    Role = vinculo.Role.Nome,
                    NomeGranja = vinculo.Granja?.NomePropriedade,
                    NomeAgroindustria = vinculo.Agroindustria?.NomeFantasia
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao ativar usuário: " + ex.Message);
            }
        }


        // Mesma lógica de GetUsuariosPendentes porém para inativos
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

                // Consulta os usuários não ativados (com vínculo cuja Role é "INATIVO")
                IQueryable<Usuario> usuariosInativosQuery = _context.Usuarios
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Role)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Granja)
                    .Include(u => u.Vinculos)
                        .ThenInclude(v => v.Agroindustria)
                    .Where(u => u.Vinculos.Any(v => v.Role.Nome == "INATIVO"));

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
                            VinculoId = v.Id,
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

        // Mesma lógica de AtivarUsuarioPendente porém pra inativos
        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPost("reativar/{id}")]
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

                // Busca o primeiro vínculo correspondente na tabela Vinculos com o status "INATIVO"
                var vinculo = await _context.Vinculos
                    .Include(v => v.Role)
                    .FirstOrDefaultAsync(v => v.UsuarioId == id && v.Role.Nome == "INATIVO");

                if (vinculo == null)
                {
                    return NotFound("Usuário não encontrado!");
                }

                if ((Role == "GESTOR_AGRO" && vinculo.AgroindustriaId != agroindustriaId) ||
                    (Role == "GESTOR_GRANJA" && vinculo.GranjaId != granjaId))
                {
                    return Forbid("Você não tem permissão para ativar este usuário.");
                }

                vinculo.RoleId = modelAtivar.RoleId;
                vinculo.GranjaId = modelAtivar.GranjaId;
                vinculo.AgroindustriaId = modelAtivar.AgroindustriaId;

                await _context.SaveChangesAsync();

                return Ok(new GetVinculoDTO
                {
                    VinculoId = vinculo.Id,
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

        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpGet("vinculos/{id}")]
        public async Task<IActionResult> GetVinculosPorUsuario(int id)
        {
            try
            {
                // Obter informações do token
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var vinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                // Obtem agroindustriaId e granjaId do vínculo do usuário autenticado
                var agroindustriaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var granjaId = _context.Vinculos
                    .Where(v => v.Id == vinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Busca os vínculos do usuário solicitado
                var vinculosQuery = _context.Vinculos
                    .Where(v => v.UsuarioId == id)
                    .Include(v => v.Role)
                    .Include(v => v.Granja)
                    .Include(v => v.Agroindustria)
                    .AsQueryable();

                // Aplica filtros de acordo com a Role do usuário autenticado
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
                        VinculoId = v.Id,
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

        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPut("vinculos/editar/{vinculoId}")]
        public async Task<IActionResult> EditarVinculo(int vinculoId, [FromBody] SetVinculoDTO modelVinculo)
        {
            try
            {
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var usuarioVinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Obtem agroindustriaId e granjaId do vínculo do usuário autenticado
                var usuarioAgroindustriaId = _context.Vinculos
                    .Where(v => v.Id == usuarioVinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var usuarioGranjaId = _context.Vinculos
                    .Where(v => v.Id == usuarioVinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Busca o vínculo a ser editado
                var vinculo = await _context.Vinculos
                    .Include(v => v.Role)
                    .Include(v => v.Granja)
                    .Include(v => v.Agroindustria)
                    .FirstOrDefaultAsync(v => v.Id == vinculoId);

                if (vinculo == null)
                {
                    return NotFound("Vínculo não encontrado!");
                }

                // Aplicar regras de edição com base na Role do usuário autenticado
                if (userRole == "GESTOR_AGRO")
                {
                    // Verificar se o vínculo pertence à mesma Agroindustria
                    if (vinculo.AgroindustriaId != usuarioAgroindustriaId)
                    {
                        return Forbid("Você só pode editar vínculos da sua própria Agroindústria.");
                    }

                    // Permitir alteração de RoleId e GranjaId, mas manter AgroindustriaId inalterado
                    vinculo.RoleId = modelVinculo.RoleId;
                    vinculo.GranjaId = modelVinculo.GranjaId ?? vinculo.GranjaId; // Preserva o valor se for nulo
                }
                else if (userRole == "GESTOR_GRANJA")
                {
                    // Verificar se o vínculo pertence à mesma Granja
                    if (vinculo.GranjaId != usuarioGranjaId)
                    {
                        return Forbid("Você só pode editar vínculos da sua própria Granja.");
                    }

                    // Permitir apenas alteração de RoleId, mantendo GranjaId e AgroindustriaId inalterados
                    vinculo.RoleId = modelVinculo.RoleId;
                }
                else if (userRole == "ADMIN")
                {
                    // ADMIN pode alterar todos os campos
                    vinculo.RoleId = modelVinculo.RoleId;
                    vinculo.GranjaId = modelVinculo.GranjaId ?? vinculo.GranjaId;
                    vinculo.AgroindustriaId = modelVinculo.AgroindustriaId ?? vinculo.AgroindustriaId;
                }
                else
                {
                    return Forbid("Permissão insuficiente para editar vínculos.");
                }

                // Salva alterações no banco de dados
                await _context.SaveChangesAsync();

                return Ok(new { message = "Vínculo atualizado com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao editar vínculo: " + ex.Message);
            }
        }

        [Authorize(Policy = "GerenciaUsuarios")]
        [HttpPost("novo-vinculo/{id}")]
        public async Task<IActionResult> AdicionarVinculoPorUsuario(int id, [FromBody] SetVinculoDTO modelVinculo)
        {
            try
            {
                // Obter informações do usuário autenticado
                var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
                var usuarioVinculoId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

                // Obter agroindustriaId e granjaId do vínculo do usuário autenticado
                var usuarioAgroindustriaId = _context.Vinculos
                    .Where(v => v.Id == usuarioVinculoId)
                    .Select(v => v.AgroindustriaId)
                    .FirstOrDefault();

                var usuarioGranjaId = _context.Vinculos
                    .Where(v => v.Id == usuarioVinculoId)
                    .Select(v => v.GranjaId)
                    .FirstOrDefault();

                // Criar um novo vínculo com base na Role do usuário autenticado
                var novoVinculo = new Vinculo
                {
                    UsuarioId = id,
                    RoleId = modelVinculo.RoleId // RoleId será preenchido em todos os casos
                };

                if (userRole == "GESTOR_AGRO")
                {
                    // GESTOR_AGRO: preenche RoleId e GranjaId do DTO e usa a própria AgroindustriaId
                    novoVinculo.GranjaId = modelVinculo.GranjaId;
                    novoVinculo.AgroindustriaId = usuarioAgroindustriaId;
                }
                else if (userRole == "GESTOR_GRANJA")
                {
                    // GESTOR_GRANJA: preenche apenas RoleId, e mantém GranjaId e AgroindustriaId do próprio vínculo
                    novoVinculo.GranjaId = usuarioGranjaId;
                    novoVinculo.AgroindustriaId = usuarioAgroindustriaId;
                }
                else if (userRole == "ADMIN")
                {
                    // ADMIN: Pode definir todos os campos vindos do DTO
                    novoVinculo.GranjaId = modelVinculo.GranjaId;
                    novoVinculo.AgroindustriaId = modelVinculo.AgroindustriaId;
                }
                else
                {
                    return Forbid("Permissão insuficiente para adicionar vínculo.");
                }

                // Adiciona o novo vínculo ao banco de dados
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