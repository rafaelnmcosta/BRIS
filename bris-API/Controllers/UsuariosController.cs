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
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
        }

        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet()]
        public async Task<IActionResult> GetUsuarios()
        {
            return Ok(await _context.Usuarios.ToListAsync());
        }

        [Authorize(Policy = "GerenciaTotal")]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] AdminCadastraUsuarioDto modelUsuario)
        {
            if (await _context.Usuarios.AnyAsync(u => u.Email == modelUsuario.Email))
                return BadRequest("Já existe um usuário com esse email!");

            var usuario = new Usuario
            {
                Nome = modelUsuario.Nome,
                Email = modelUsuario.Email,
                CPF = modelUsuario.CPF,
                AgroindustriaId = modelUsuario.AgroindustriaId
            };

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            var salt = PasswordService.GenerateSalt();
            var hash = PasswordService.HashPassword(modelUsuario.Senha, salt);

            var senha = new Senha
            {
                UsuarioId = usuario.Id,
                SenhaHash = hash,
                Salt = salt
            };

            _context.Senhas.Add(senha);
            await _context.SaveChangesAsync();

            var granjaUsuarioTipo = new Vinculos
            {
                UsuarioId = usuario.Id,
                GranjaId = modelUsuario.GranjaId,
                TipoUsuarioId = modelUsuario.TipoUsuarioId
            };

            _context.GranjasUsuariosTipos.Add(granjaUsuarioTipo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        [Authorize(Policy = "VisualizaTotal")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.GranjasUsuariosTipos)
                    .ThenInclude(gut => gut.TipoUsuario)
                .Include(u => u.GranjasUsuariosTipos)
                    .ThenInclude(gut => gut.Granja)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound();
            }

            var response = new
            {
                Usuario = usuario,
                Acessos = usuario.GranjasUsuariosTipos.Select(gut => new 
                {
                    gut.Id,
                    gut.TipoUsuarioId,
                    gut.TipoUsuario?.Tipo,
                    gut.GranjaId,
                    gut.Granja?.NomePropriedade
                }).ToList()
            };

            return Ok(response);
        }


        [Authorize(Policy = "GerenciaTotal")]
        [HttpGet("ativar")]
        public async Task<IActionResult> GetUsuariosNaoAtivados()
        {
            // Buscar todos os acessos em GranjasUsuariosTipos com TipoUsuarioId igual a 98
            var usuariosNaoAtivadosIds = await _context.GranjasUsuariosTipos
                .Where(gut => gut.TipoUsuarioId == 98)
                .Select(gut => gut.UsuarioId)
                .Distinct()
                .ToListAsync();

            // Buscar todos os usuários correspondentes na tabela Usuarios
            var usuariosNaoAtivados = await _context.Usuarios
                .Where(u => usuariosNaoAtivadosIds.Contains(u.Id))
                .ToListAsync();

            return Ok(usuariosNaoAtivados);
        }

        [Authorize(Policy = "GerenciaTotal")]
        [HttpPost("ativar/{id}")]
        public async Task<IActionResult> AtivarUsuario(int id, [FromBody] AtivarDto ativar)
        {
            // Buscar o primeiro acesso correspondente na tabela GranjasUsuariosTipos
            var acesso = await _context.GranjasUsuariosTipos
                .FirstOrDefaultAsync(gut => gut.UsuarioId == id && gut.TipoUsuarioId == 98);

            if (acesso == null)
            {
                return NotFound(); // Se o acesso não for encontrado, retornar NotFound
            }

            // Atualizar os campos com os valores do DTO
            acesso.TipoUsuarioId = ativar.TipoUsuario;
            acesso.GranjaId = ativar.GranjaId;

            // Salvar as alterações no banco de dados
            await _context.SaveChangesAsync();

            // Retornar o acesso atualizado
            return Ok(acesso);
        }


        [Authorize(Policy = "GerenciaTotal")]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] AdminEditaUsuario modelUsuario)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            // Atualiza as informações do usuário
            usuario.Nome = modelUsuario.Nome;
            usuario.Email = modelUsuario.Email;
            usuario.CPF = modelUsuario.CPF;
            usuario.AgroindustriaId = modelUsuario.AgroindustriaId;

            // Atualiza a senha
            var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == id);
            if (senha != null)
            {
                var salt = PasswordService.GenerateSalt();
                senha.SenhaHash = PasswordService.HashPassword(modelUsuario.Senha, salt);
                senha.Salt = salt;
            }

            // Atualiza ou adiciona os acessos em GranjasUsuariosTipos
            foreach (var nivelAcesso in modelUsuario.NiveisAcesso)
            {
                var acesso = await _context.GranjasUsuariosTipos
                    .FirstOrDefaultAsync(gut => gut.Id == nivelAcesso.Id && gut.UsuarioId == id);

                if (acesso != null)
                {
                    acesso.TipoUsuarioId = nivelAcesso.TipoUsuarioId;
                    acesso.GranjaId = nivelAcesso.GranjaId ?? acesso.GranjaId;
                }
                else
                {
                    // Lógica para adicionar um novo acesso, se necessário
                    var novoacesso = new Vinculos
                    {
                        UsuarioId = id,
                        TipoUsuarioId = nivelAcesso.TipoUsuarioId,
                        GranjaId = nivelAcesso.GranjaId ?? default(int)
                    };
                    _context.GranjasUsuariosTipos.Add(novoacesso);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário e níveis de acesso atualizados com sucesso!" });
        }
    }
}