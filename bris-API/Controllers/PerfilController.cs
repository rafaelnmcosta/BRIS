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

        [Authorize(Roles = PoliticasDeAcesso.TodosUsuarios)]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPerfil(int id)
        {
            // Extraindo o GranjaUsuarioTipoId do token JWT
            var granjaUsuarioTipoId = User.Claims.FirstOrDefault(c => c.Type == "GranjaUsuarioTipoId")?.Value;
            if (granjaUsuarioTipoId == null)
            {
                return Unauthorized("Token inválido ou expirado.");
            }

            // Buscando o registro de GranjasUsuariosTipos correspondente ao token
            var granjaUsuarioTipo = await _context.GranjasUsuariosTipos
                .Include(gut => gut.TipoUsuario)
                .Include(gut => gut.Granja)
                .FirstOrDefaultAsync(gut => gut.Id.ToString() == granjaUsuarioTipoId);

            if (granjaUsuarioTipo == null)
            {
                return Unauthorized("Token inválido ou não autorizado.");
            }

            // Verificando se o ID solicitado corresponde ao usuário logado
            if (granjaUsuarioTipo.UsuarioId != id)
            {
                return Forbid("Você não tem permissão para acessar o perfil de outro usuário.");
            }

            // Buscando o usuário
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            // Construindo a resposta com os dados do usuário, tipo de usuário e granja
            var resultado = new
            {
                Usuario = usuario,
                TipoUsuario = granjaUsuarioTipo.TipoUsuario?.Tipo,
                Granja = granjaUsuarioTipo.Granja?.NomePropriedade
            };

            return Ok(resultado);
        }

        [Authorize(Roles = PoliticasDeAcesso.TodosUsuarios)]
        [HttpPut("{id}/editar")]
        public async Task<IActionResult> EditarPerfil(int id, [FromBody] CadastroDto cadastroDto)
        {
            var userIdFromToken = User.FindFirstValue("UsuarioId");
            if (userIdFromToken == null || int.Parse(userIdFromToken) != id)
            {
                return Forbid();
            }

            var usuario = await _context.Usuarios.FindAsync(id);
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

                var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == id);
                if (senha != null)
                {
                    senha.SenhaHash = hash;
                    senha.Salt = salt;
                }
                else
                {
                    senha = new Senha
                    {
                        UsuarioId = id,
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