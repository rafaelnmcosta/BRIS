using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class AdminEditaAnimalDto
    {
        [Required(ErrorMessage = "A linhagem é obrigatória.")]
        [StringLength(200, ErrorMessage = "A linhagem deve ter no máximo 200 caracteres.")]
        public required string Linhagem { get; set; }

        [Required(ErrorMessage = "A idade é obrigatória.")]
        [Range(1, int.MaxValue, ErrorMessage = "A idade deve ser um valor positivo.")]
        public required int Idade { get; set; }

        [Required(ErrorMessage = "O peso é obrigatório.")]
        [Range(1, float.MaxValue, ErrorMessage = "O peso deve ser um valor positivo.")]
        public required int Peso { get; set; }

        [Required(ErrorMessage = "O id da agroindustria é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id da agroindustria deve ser um valor positivo.")]
        public required int AgroindustriaId { get; set; }

        [Required(ErrorMessage = "O id da granja é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id da granja deve ser um valor positivo.")]
        public required int GranjaId { get; set; }

        // O status inicia como nulo e só receberá um valor após algumas regras de negócio de avaliação serem executadas
        public bool? status { get; set; }

        [Required(ErrorMessage = "O estado de ativação é obrigatório.")]
        public required bool Ativo { get; set; }
    }
}