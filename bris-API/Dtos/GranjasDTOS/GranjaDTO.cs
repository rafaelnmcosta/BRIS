using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class GranjaDTO
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public required string NomePropriedade { get; set; }


        [Required(ErrorMessage = "O id da agroindustria é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id da agroindustria deve ser um valor positivo.")]
        public required int AgroindustriaId { get; set; }


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