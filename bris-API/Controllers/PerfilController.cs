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
    [Route("api/perfil")]
    [ApiController]
    public class PerfilController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PerfilController(AppDbContext context)
        {
            _context = context;
        }

        [Authorize(Policy = "TodosUsuarios")]
        [HttpGet()]
        public async Task<IActionResult> GetPerfil()
        {
            // Extraindo o UsuarioId do token JWT
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var tipoUsuario = User.FindFirstValue(ClaimTypes.Role);

            // Buscando o usuário
            var usuario = await _context.Usuarios
                .Include(u => u.Agroindustria)
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }
            
            // Buscando o registro de GranjasUsuariosTipos correspondente
            var granjaUsuarioTipo = await _context.GranjasUsuariosTipos
                .Include(gut => gut.TipoUsuario)
                .Include(gut => gut.Granja)
                .Include(gut => gut.Usuario)
                .FirstOrDefaultAsync(gut => gut.UsuarioId == usuarioId && gut.TipoUsuario.Tipo == tipoUsuario);
        
            if (granjaUsuarioTipo == null)
            {
                return Unauthorized("Usuário não autorizado ou não associado a uma granja.");
            }

            // Construindo a resposta
            var dadosPerfil = new PerfilDto
            {
                Nome = usuario.Nome,
                Email = usuario.Email,
                CPF = usuario.CPF,
                TipoUsuario = tipoUsuario,
                NomeAgroindustria = usuario.Agroindustria.NomeFantasia,
                NomeGranja = (granjaUsuarioTipo.TipoUsuario.Tipo == "TECNICO" || granjaUsuarioTipo.TipoUsuario.Tipo == "GESTOR_GRANJA")
                            ? granjaUsuarioTipo.Granja?.NomePropriedade
                            : null
            };


            return Ok(dadosPerfil);
        }

        [Authorize(Policy = "TodosUsuarios")]
        [HttpPut("editar")]
        public async Task<IActionResult> EditarPerfil([FromBody] CadastroDto cadastroDto)
        {
            var usuarioId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            

            var usuario = await _context.Usuarios.FindAsync(usuarioId);
            if (usuario == null)
            {
                return NotFound();
            }

            // Atualiza as informações do usuário
            usuario.Nome = cadastroDto.Nome;
            usuario.Email = cadastroDto.Email;
            usuario.CPF = cadastroDto.CPF;

            // Atualiza a senha, se fornecida
            if (!string.IsNullOrEmpty(cadastroDto.Senha))
            {
                var salt = PasswordService.GenerateSalt();
                var hash = PasswordService.HashPassword(cadastroDto.Senha, salt);

                var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == usuarioId);
                if (senha != null)
                {
                    senha.SenhaHash = hash;
                    senha.Salt = salt;
                }
                else
                {
                    senha = new Senha
                    {
                        UsuarioId = usuarioId,
                        SenhaHash = hash,
                        Salt = salt
                    };
                    _context.Senhas.Add(senha);
                }
            }

            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Perfil atualizado com sucesso!" });
        }
    }
}
