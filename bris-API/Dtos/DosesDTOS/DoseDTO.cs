using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class DoseDto
    {

        [Required(ErrorMessage = "O valor registrado é obrigatório.")]
        [Range(1, float.MaxValue, ErrorMessage = "O valor registrado deve ser um valor positivo.")]
        public required float? ValorRegistrado { get; set; }
    }
}