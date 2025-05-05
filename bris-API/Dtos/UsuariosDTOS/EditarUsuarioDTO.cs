using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class EditarUsuarioDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter exatamente 11 dígitos numéricos.")]
        public required string CPF { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "Telefone deve conter 11 dígitos numéricos.")]
        public string? Telefone { get; set; }

        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public string? Senha { get; set; }

        public List<SetVinculoDTO>? Vinculos { get; set; }
    }
}