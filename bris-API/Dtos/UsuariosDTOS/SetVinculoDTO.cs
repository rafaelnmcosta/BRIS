using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class SetVinculoDTO
    {
        [Required(ErrorMessage = "O ID da função é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "ID da função inválido.")]
        public int RoleId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ID da agroindústria inválido.")]
        public int? AgroindustriaId { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "ID da granja inválido.")]
        public int? GranjaId { get; set; }
    }
}