using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{

    public class NivelAcessoDto
    {
        [Required(ErrorMessage = "O id do registro é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id do registro do nivel de acesso deve ser um valor positivo.")]
        public int Id { get; set; } // Id do registro em GranjasUsuariosTipos

        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id do tipo de usuário deve ser um valor positivo.")]
        public int TipoUsuarioId { get; set; } // Novo TipoUsuarioId

        [Required(ErrorMessage = "O id da agroindustria é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id da agroindustria deve ser um valor positivo.")]
        public required int AgroindustriaId { get; set; }
        public string? NomeTipoUsuario { get; set; } // Nome do TipoUsuario
        public int? GranjaId { get; set; } // Granja associada
        public string? NomeGranja { get; set; } // Nome da Granja
    }
}
