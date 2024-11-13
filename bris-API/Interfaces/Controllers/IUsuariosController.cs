using Microsoft.AspNetCore.Mvc;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    public interface IUsuariosController
    {
        Task<IActionResult> GetUsuarios();
        Task<IActionResult> CadastrarUsuario([FromBody] CadastrarUsuarioDTO modelUsuario);
        Task<IActionResult> GetUsuarioPorId(int id);
        Task<IActionResult> EditarUsuario(int id, [FromBody] EditarUsuarioDTO modelUsuario);
        Task<IActionResult> GetUsuariosPendentes();
        Task<IActionResult> AtivarUsuarioPendente(int id, [FromBody] AtivarUsuarioDto modelAtivar);
        Task<IActionResult> GetUsuariosInativos();
        Task<IActionResult> ReativarUsuarioInativo(int id, [FromBody] AtivarUsuarioDto modelAtivar);
        Task<IActionResult> GetVinculosPorUsuario(int id);
        Task<IActionResult> EditarVinculo(int vinculoId, [FromBody] SetVinculoDTO modelVinculo);
        Task<IActionResult> AdicionarVinculoPorUsuario(int id, [FromBody] SetVinculoDTO modelVinculo);
    }
}
