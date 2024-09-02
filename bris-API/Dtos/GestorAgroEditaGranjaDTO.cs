using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class GestorAgroEditaGranjaDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public required string NomePropriedade { get; set; }
        
        [Required(ErrorMessage = "O endereço é obrigatório.")]
        [StringLength(300, ErrorMessage = "O endereço deve ter no máximo 300 caracteres.")]
        public required string Endereco { get; set; }

        [Required(ErrorMessage = "O CNPJ é obrigatório.")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "O CNPJ deve ter 14 dígitos.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "O CNPJ deve conter apenas números.")]
        public required string CNPJ { get; set; }

        [Required(ErrorMessage = "O estado de ativação é obrigatório.")]
        public required bool Ativo { get; set; }
    }
}