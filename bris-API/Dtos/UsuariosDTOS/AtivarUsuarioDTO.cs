using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class AtivarDto
    {
        [Required(ErrorMessage = "A role do usuário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "A role do usuário deve ser um valor positivo.")]
        public int Role { get; set; }
        public int? GranjaId { get; set; }
        public int? AgroindustriaId { get; set; }
    }
}