using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{

    public class VinculoDTO
    {
        [Required(ErrorMessage = "O id do vinculo é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id do vinculo deve ser um valor positivo.")]
        public int VinculoId { get; set; }

        [Required(ErrorMessage = "O id da Role é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id da Role deve ser um valor positivo.")]
        public int RoleId { get; set; }
        
        [Required(ErrorMessage = "A Role é obrigatória.")]
        public required string Role { get; set; }
        public int? GranjaId { get; set; }
        public string? NomeGranja { get; set; }
        public int? AgroindustriaId { get; set; }
        public string? NomeAgroindustria { get; set; }
    }
}
