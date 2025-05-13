using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class AgroindustriaDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public required string NomeFantasia { get; set; }

        [Required(ErrorMessage = "A razão social é obrigatória.")]
        [StringLength(100, ErrorMessage = "A razão social deve ter no máximo 100 caracteres.")]
        public required string RazaoSocial { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 dígitos.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve conter apenas números.")]
        public required string CNPJ { get; set; }

        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public string? Email { get; set; }

        [RegularExpression(@"^\d{11}$", ErrorMessage = "O telefone deve conter 11 dígitos numéricos.")]
        public string? Telefone { get; set; }

    }
}