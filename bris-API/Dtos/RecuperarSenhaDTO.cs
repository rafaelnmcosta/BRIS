using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class RecuperarSenhaDto
    {

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public required string Email { get; set; }
    }
}