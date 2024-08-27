using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class AtivarDto
    {
        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O tipo de usuário deve ser um valor positivo.")]
        public int TipoUsuario { get; set; }
    }
}