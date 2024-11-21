using Microsoft.AspNetCore.Mvc;
using bris_API.DTOs;

namespace bris_API.Controllers
{
    public interface IAutenticacaoController
    {
        Task<IActionResult> Cadastro(AutoCadastroDTO modelUsuario);
        Task<IActionResult> Login(LoginDto modelLogin);
        Task<IActionResult> Logout();
        Task<IActionResult> GetVinculos();
        Task<IActionResult> SelecionarVinculo(int id);
        Task<IActionResult> TrocarVinculo();
        Task<IActionResult> ProcessarRecuperacaoSenha(RecuperarSenhaDto model);
        public IActionResult CheckStatus();
    }
}
