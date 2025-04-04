using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class CadastrarUsuarioDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números.")]
        public required string CPF { get; set; }

        [Phone(ErrorMessage = "Número de telefone inválido.")]
        public string? Telefone { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public required string Senha { get; set; }

        [Required(ErrorMessage = "Pelo menos um vínculo deve ser informado.")]
        [MinLength(1, ErrorMessage = "Deve haver pelo menos um vínculo.")]
        public List<VinculoDTO> Vinculos { get; set; } = new List<VinculoDTO>();
    }
}