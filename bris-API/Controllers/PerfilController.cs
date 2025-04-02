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
    /// Controller responsável por gerenciar as operações relacionadas ao perfil do usuário.
    /// Permite consultar e atualizar as informações do perfil do usuário autenticado.
    /// </summary>
    [Route("api/perfil")]
    [ApiController]
    public class PerfilController : ControllerBase, IPerfilController
    {
        private readonly AppDbContext _context;
        private readonly IPasswordService _passwordService;

        /// <summary>
        /// Construtor que injeta as dependências necessárias.
        /// </summary>
        /// <param name="context">Contexto do banco de dados.</param>
        /// <param name="passwordService">Serviço para manipulação de senhas (hash, salt, etc.).</param>
        public PerfilController(AppDbContext context, IPasswordService passwordService)
        {
            _context = context;
            _passwordService = passwordService;
        }

        /// <summary>
        /// Obtém as informações do perfil do usuário autenticado.
        /// A consulta utiliza o vínculo do usuário (obtido através do token) para recuperar informações
        /// como Nome, Email, CPF e informações relacionadas ao vínculo (Role, Nome da Agroindústria e Nome da Granja).
        /// </summary>
        /// <returns>Um objeto do tipo GetPerfilDTO contendo os dados do perfil.</returns>
        [Authorize(Policy = "TodosUsuarios")]
        [HttpGet()]
        public async Task<IActionResult> GetPerfil()
        {
            try
            {
                // Obtém o ID do vínculo presente no token do usuário autenticado.
                var vinculoId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
    
                // Busca o vínculo associado, incluindo as entidades relacionadas (Role, Granja, Agroindustria e Usuario)
                var vinculo = await _context.Vinculos
                    .Include(v => v.Role)
                    .Include(v => v.Granja)
                    .Include(v => v.Agroindustria)
                    .Include(v => v.Usuario)
                    .FirstOrDefaultAsync(v => v.Id == int.Parse(vinculoId));
    
                // Mapeia os dados para o DTO de perfil
                var dadosPerfil = new GetPerfilDTO
                {
                    Nome = vinculo.Usuario.Nome,
                    Email = vinculo.Usuario.Email,
                    CPF = vinculo.Usuario.CPF,
                    Role = vinculo.Role.Nome,
                    // Se a role do usuário for ADMIN, não há vínculo com Agroindústria; caso contrário, exibe o NomeFantasia
                    NomeAgroindustria = (vinculo.Role.Nome == "ADMIN") ? null : vinculo.Agroindustria.NomeFantasia,
                    // Se a role do usuário for ADMIN ou GESTOR_AGRO, não há vínculo com Granja; caso contrário, exibe o Nome da Propriedade
                    NomeGranja = (vinculo.Role.Nome == "ADMIN" || vinculo.Role.Nome == "GESTOR_AGRO") ? null : vinculo.Granja.NomePropriedade
                };
    
                return Ok(dadosPerfil);
            }
            catch (Exception ex)
            {
                // Em caso de erro, retorna status 500 com mensagem detalhada.
                return StatusCode(500, "Erro ao acessar perfil: " + ex.Message);
            }
        }

        /// <summary>
        /// Atualiza as informações do perfil do usuário autenticado.
        /// Permite atualizar Nome, Email, CPF e, opcionalmente, a senha.
        /// Se uma nova senha for fornecida, o sistema gera um novo salt e hash.
        /// </summary>
        /// <param name="modelPerfil">Dados atualizados do perfil, encapsulados em EditarPerfilDTO.</param>
        /// <returns>Mensagem de sucesso ou erro.</returns>
        [Authorize(Policy = "TodosUsuarios")]
        [HttpPut("editar")]
        public async Task<IActionResult> EditarPerfil([FromBody] EditarPerfilDTO modelPerfil)
        {
            try
            {
                // Obtém o ID do vínculo do usuário autenticado a partir do token.
                var vinculoId = User.FindFirst(ClaimTypes.NameIdentifier).Value;
    
                // Busca o vínculo do usuário, incluindo o usuário associado.
                var vinculo = await _context.Vinculos
                    .Include(v => v.Usuario)
                    .FirstOrDefaultAsync(v => v.Id == int.Parse(vinculoId));
                
                // Recupera o usuário associado ao vínculo
                var usuario = vinculo.Usuario;
    
                // Atualiza os campos básicos do usuário com os valores do DTO.
                // Se algum campo não for fornecido, mantém o valor anterior.
                usuario.Nome = modelPerfil.Nome;
                usuario.Email = modelPerfil.Email;
                usuario.CPF = modelPerfil.CPF;
    
                // Se uma nova senha for fornecida, gera novo salt e hash para atualizá-la.
                if (!string.IsNullOrEmpty(modelPerfil.Senha))
                {
                    var salt = _passwordService.GenerateSalt();
                    var hash = _passwordService.HashPassword(modelPerfil.Senha, salt);
    
                    // Busca o registro de senha correspondente ao usuário.
                    var senha = await _context.Senhas.FirstOrDefaultAsync(s => s.UsuarioId == usuario.Id);
                    if (senha != null)
                    {
                        senha.SenhaHash = hash;
                        senha.Salt = salt;
                    }
                    else
                    {
                        // Caso não exista, cria um novo registro de senha.
                        senha = new Senha
                        {
                            UsuarioId = usuario.Id,
                            SenhaHash = hash,
                            Salt = salt
                        };
                        _context.Senhas.Add(senha);
                    }
                }
    
                // Atualiza o usuário no banco de dados.
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
