using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

using bris_API.Data;
using bris_API.Models;
using bris_API.Services;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public UsuariosController(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet()]
        public async Task<IActionResult> GetUsuarios()
        {
            try
            {
                var usuarios = await _context.Usuarios.ToListAsync();
                return Ok(usuarios);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar usuários: " + ex.Message);
            }
        }
        [Authorize(Policy = "GerenciaTotal")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CadastrarUsuarioDTO modelUsuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == modelUsuario.Email))
                return BadRequest("Já existe um usuário com esse email!");

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

        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            try
            {
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

                var usuarioDTO = new GetUsuarioDTO
                {
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    CPF = usuario.CPF,
                    Vinculos = usuario.Vinculos.Select(v => new VinculoDTO
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

        [Authorize(Policy = "GerenciaTotal")]
        [HttpGet("ativar")]
        public async Task<IActionResult> GetUsuariosNaoAtivados()
        {
            try
            {
                // Buscar todos os vinculos com RoleId igual a 98
                var usuariosNaoAtivadosIds = await _context.Vinculos
                    .Where(v => v.RoleId == 98)
                    .Select(v => v.UsuarioId)
                    .Distinct()
                    .ToListAsync();

                // Buscar todos os usuários correspondentes na tabela Usuarios
                var usuariosNaoAtivados = await _context.Usuarios
                    .Where(u => usuariosNaoAtivadosIds.Contains(u.Id))
                    .ToListAsync();

                return Ok(usuariosNaoAtivados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao buscar usuários não ativados: " + ex.Message);
            }
        }

        [Authorize(Policy = "GerenciaTotal")]
        [HttpPost("ativar/{id}")]
        public async Task<IActionResult> AtivarUsuario(int id, [FromBody] AtivarDto modelAtivar)
        {
            try
            {    
                // Busca o primeiro vinculo correspondente na tabela Vinculos (um usuário não pode ter mais de um vínculo "pendente")
                var vinculo = await _context.Vinculos
                    .FirstOrDefaultAsync(v => v.UsuarioId == id && v.RoleId == 98);

                if (vinculo == null)
                {
                    return NotFound("Usuário não encontrado!");
                }
                
                vinculo.RoleId = modelAtivar.Role;
                vinculo.GranjaId = modelAtivar.GranjaId;
                vinculo.AgroindustriaId = modelAtivar.AgroindustriaId;

                await _context.SaveChangesAsync();

                // Retorna o vinculo atualizado
                return Ok(vinculo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao ativar usuário: " + ex.Message);
            }
        }

        [Authorize(Policy = "GerenciaTotal")]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] EditarUsuarioDTO modelUsuario)
        {
            try
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound("Usuário não encontrado!");
                }

                usuario.Nome = modelUsuario.Nome;
                usuario.Email = modelUsuario.Email;
                usuario.CPF = modelUsuario.CPF;

                var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == id);
                if (senha != null)
                {
                    var salt = _passwordService.GenerateSalt();
                    senha.SenhaHash = _passwordService.HashPassword(modelUsuario.Senha, salt);
                    senha.Salt = salt;
                }

                // Atualiza ou adiciona vínculos
                foreach (var vinculoDTO in modelUsuario.Vinculos)
                {
                    var vinculo = await _context.Vinculos
                        .FirstOrDefaultAsync(v => v.Id == vinculoDTO.VinculoId && v.UsuarioId == id);

                    if (vinculo != null)
                    {
                        vinculo.RoleId = vinculoDTO.RoleId;
                        vinculo.GranjaId = vinculoDTO.GranjaId ?? vinculo.GranjaId;
                        vinculo.AgroindustriaId = vinculoDTO.AgroindustriaId ?? vinculo.AgroindustriaId;
                    }
                    else
                    {
                        // adiciona um novo vinculo se precisar
                        var novoVinculo = new Vinculo
                        {
                            UsuarioId = id,
                            RoleId = vinculoDTO.RoleId,
                            GranjaId = vinculoDTO.GranjaId ?? default,
                            AgroindustriaId = vinculoDTO.AgroindustriaId ?? default
                        };
                        _context.Vinculos.Add(novoVinculo);
                    }
                }

                await _context.SaveChangesAsync();
                return Ok(new { message = "Usuário e vínculos atualizados com sucesso!" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao editar usuário: " + ex.Message);
            }
        }
    }
}