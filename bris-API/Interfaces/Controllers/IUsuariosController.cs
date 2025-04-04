using Microsoft.AspNetCore.Mvc;
using bris_API.DTOs;
using System.Collections.Generic;

namespace bris_API.Controllers
{
    public interface IUsuariosController
    {
        Task<IActionResult> GetUsuarios();
        Task<IActionResult> CadastrarUsuario([FromBody] CadastrarUsuarioDTO modelUsuario);
        Task<IActionResult> GetUsuarioPorId(int id);
        Task<IActionResult> EditarUsuario(int id, [FromBody] EditarUsuarioDTO modelUsuario);
        Task<IActionResult> InativarUsuario(int id);
        Task<IActionResult> GetUsuariosInativos();
        Task<IActionResult> ReativarUsuarioInativo(int id, [FromBody] List<VinculoDTO> novosVinculos);
        Task<IActionResult> GetVinculosPorUsuario(int id);
    }
}