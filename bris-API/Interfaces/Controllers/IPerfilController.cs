// IPerfilController.cs
using Microsoft.AspNetCore.Mvc;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    public interface IPerfilController
    {
        Task<IActionResult> GetPerfil();
        Task<IActionResult> EditarPerfil([FromBody] EditarPerfilDTO modelPerfil);
    }
}
