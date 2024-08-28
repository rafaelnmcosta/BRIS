using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class CadastroAnimalDto
    {
        [Required(ErrorMessage = "A linhagem é obrigatória.")]
        [StringLength(100, ErrorMessage = "As informações da linhagem devem ter no máximo 100 caracteres.")]
        public string? Linhagem { get; set; }

        [Required(ErrorMessage = "A idade é obrigatória.")]
        [Range(0, 100, ErrorMessage = "A idade informada não é válida (0 a 100).")]
        public int Idade { get; set; }

        [Required(ErrorMessage = "O peso é obrigatório.")]
        [Range(0, float.MaxValue, ErrorMessage = "O peso não é válido.")]
        public float Peso { get; set; }
    }
}