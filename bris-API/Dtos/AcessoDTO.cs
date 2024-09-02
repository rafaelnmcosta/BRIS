using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class AcessoDTO
    {
        [Required(ErrorMessage = "O id do registro é obrigatório.")]
        public required int Id { get; set; }

        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        public required string NomeTipo { get; set; }

        [Required(ErrorMessage = "O id do tipo de usuário é obrigatório.")]
        public required int TipoId { get; set; }

        public string? NomeGranja { get; set; }
        
        public int? GranjaId { get; set; }
    }
}
