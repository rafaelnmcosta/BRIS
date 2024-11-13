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
    public class PerfilController : ControllerBase, IPerfilController
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        public PerfilController(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        [Authorize(Policy = "TodosUsuarios")]
        [HttpGet()]
        public async Task<IActionResult> GetPerfil()
        {
            try
            {
                var vinculoId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
    
                var vinculo = await _context.Vinculos
                    .Include(v => v.Role)
                    .Include(v => v.Granja)
                    .Include(v => v.Agroindustria)
                    .Include(v => v.Usuario)
                    .FirstOrDefaultAsync(v => v.Id == int.Parse(vinculoId));
    
                var dadosPerfil = new GetPerfilDTO
                {
                    Nome = vinculo.Usuario.Nome,
                    Email = vinculo.Usuario.Email,
                    CPF = vinculo.Usuario.CPF,
                    Role = vinculo.Role.Nome,
                    NomeAgroindustria = (vinculo.Role.Nome == "ADMIN") ? null : vinculo.Agroindustria.NomeFantasia,
                    NomeGranja = (vinculo.Role.Nome == "ADMIN" || vinculo.Role.Nome == "GESTOR_AGRO") ? null : vinculo.Granja.NomePropriedade
                };
    
    
                return Ok(dadosPerfil);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao acessar perfil: " + ex.Message);
            }
        }

        [Authorize(Policy = "TodosUsuarios")]
        [HttpPut("editar")]
        public async Task<IActionResult> EditarPerfil([FromBody] EditarPerfilDTO modelPerfil)
        {
            try
            {
                var vinculoId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
    
                var vinculo = await _context.Vinculos
                    .Include(v => v.Usuario)
                    .FirstOrDefaultAsync(v => v.Id == int.Parse(vinculoId));
                
                // Seleciona o usuário referente ao vínculo
                var usuario = vinculo.Usuario;
    
                // Atualiza as informações do usuário com os valores do DTO (se não preechidos no front, virão com valor anterior)
                usuario.Nome = modelPerfil.Nome;
                usuario.Email = modelPerfil.Email;
                usuario.CPF = modelPerfil.CPF;
    
                // Atualiza a senha, se fornecida
                if (!string.IsNullOrEmpty(modelPerfil.Senha))
                {
                    var salt = _passwordService.GenerateSalt();
                    var hash = _passwordService.HashPassword(modelPerfil.Senha, salt);
    
                    var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == usuario.Id);
                    if (senha != null)
                    {
                        senha.SenhaHash = hash;
                        senha.Salt = salt;
                    }
                    else
                    {
                        senha = new Senha
                        {
                            UsuarioId = usuario.Id,
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
            catch (Exception ex)
            {
                return StatusCode(500, "Erro ao editar perfil: " + ex.Message);
            }
        }
    }
}
