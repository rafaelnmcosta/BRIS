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
    [Route("api/users")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsuariosController(AppDbContext context, ITokenService tokenService)
        {
            _context = context;
        }

        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoTotal)]
        [HttpGet()]
        public async Task<IActionResult> GetUsuarios()
        {
            return Ok(await _context.Usuarios.ToListAsync());
        }

        [Authorize(Roles = PoliticasDeAcesso.GerenciaTotal)]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> CadastrarUsuario([FromBody] CadastroAdminDto modelUsuario)
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

            var granjaUsuarioTipo = new GranjaUsuarioTipo
            {
                UsuarioId = usuario.Id,
                GranjaId = modelUsuario.GranjaId,
                TipoUsuarioId = modelUsuario.TipoUsuarioId
            };

            _context.GranjasUsuariosTipos.Add(granjaUsuarioTipo);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Usuário registrado com sucesso!" });
        }

        [Authorize(Roles = PoliticasDeAcesso.VisualizacaoTotal)]
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


        [Authorize(Roles = PoliticasDeAcesso.GerenciaTotal)]
        [HttpGet("ativar")]
        public async Task<IActionResult> GetUsuariosNaoAtivados()
        {
            // Buscar todos os registros em GranjasUsuariosTipos com TipoUsuarioId igual a 98
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

        [Authorize(Roles = PoliticasDeAcesso.GerenciaTotal)]
        [HttpPost("ativar/{id}")]
        public async Task<IActionResult> AtivarUsuario(int id, [FromBody] AtivarDto ativar)
        {
            // Buscar o primeiro registro correspondente na tabela GranjasUsuariosTipos
            var registro = await _context.GranjasUsuariosTipos
                .FirstOrDefaultAsync(gut => gut.UsuarioId == id && gut.TipoUsuarioId == 98);

            if (registro == null)
            {
                return NotFound(); // Se o registro não for encontrado, retornar NotFound
            }

            // Atualizar os campos com os valores do DTO
            registro.TipoUsuarioId = ativar.TipoUsuario;
            registro.GranjaId = ativar.GranjaId;

            // Salvar as alterações no banco de dados
            await _context.SaveChangesAsync();

            // Retornar o registro atualizado
            return Ok(registro);
        }


        [Authorize(Roles = PoliticasDeAcesso.GerenciaTotal)]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> EditarUsuario(int id, [FromBody] EditarAdminDto modelUsuario)
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

            // Atualiza ou adiciona os registros em GranjasUsuariosTipos
            foreach (var nivelAcesso in modelUsuario.NiveisAcesso)
            {
                var registro = await _context.GranjasUsuariosTipos
                    .FirstOrDefaultAsync(gut => gut.Id == nivelAcesso.Id && gut.UsuarioId == id);

                if (registro != null)
                {
                    registro.TipoUsuarioId = nivelAcesso.TipoUsuarioId;
                    registro.GranjaId = nivelAcesso.GranjaId ?? registro.GranjaId;
                }
                else
                {
                    // Lógica para adicionar um novo registro, se necessário
                    var novoRegistro = new GranjaUsuarioTipo
                    {
                        UsuarioId = id,
                        TipoUsuarioId = nivelAcesso.TipoUsuarioId,
                        GranjaId = nivelAcesso.GranjaId ?? default(int)
                    };
                    _context.GranjasUsuariosTipos.Add(novoRegistro);
                }
            }

            await _context.SaveChangesAsync();
            return Ok(new { message = "Usuário e níveis de acesso atualizados com sucesso!" });
        }
    }
}