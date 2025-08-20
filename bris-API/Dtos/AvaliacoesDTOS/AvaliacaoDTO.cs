using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class AvaliacaoDto
    {

        [Required(ErrorMessage = "O id do Animal é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id do animal deve ser um valor positivo.")]
        public required int AnimalId { get; set; }

        [Required(ErrorMessage = "O Status da avaliação é obrigatório.")]
        [Range(1, 3, ErrorMessage = "O id do animal deve ser um valor entre 1 e 3.")]
        public required int StatusAvaliacao { get; set; }
        
        public bool? ResultadoFinal { get; set; }
        [Required(ErrorMessage = "A próxima dose é obrigatória.")]
        public required ProximaDoseDto proximaDose { get; set; }
    }
    public class ProximaDoseDto{

        [Required(ErrorMessage = "O número da semana é obrigatório.")]
        [Range(1, 5, ErrorMessage = "O número da semana deve ser um valor entre 1 e 5.")]        
        public int NroSemana { get; set; }

        [Required(ErrorMessage = "A ordem da dose é obrigatório.")]
        [Range(1, 3, ErrorMessage = "A ordem da dose deve ser um valor entre 1 e 3.")]   
        public int OrdemDose { get; set; }
    }
}