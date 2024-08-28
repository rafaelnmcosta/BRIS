using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class EditarAdminDto
    {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [StringLength(100, ErrorMessage = "O nome deve ter no máximo 100 caracteres.")]
        public required string Nome { get; set; }

        [Required(ErrorMessage = "O email é obrigatório.")]
        [EmailAddress(ErrorMessage = "O email informado não é válido.")]
        public required string Email { get; set; }

        [Required(ErrorMessage = "O tipo de usuário é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id do tipo de usuário deve ser um valor positivo.")]
        public int TipoUsuarioId { get; set; }
        
        [Required(ErrorMessage = "O id da agroindustria é obrigatório.")]
        [Range(1, int.MaxValue, ErrorMessage = "O id da agroindustria deve ser um valor positivo.")]
        public required int AgroindustriaId { get; set; }

        public int? GranjaId { get; set; }

        [Required(ErrorMessage = "O CPF é obrigatório.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "O CPF deve ter 11 dígitos.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "O CPF deve conter apenas números.")]
        public required string CPF { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "A senha deve ter entre 6 e 100 caracteres.")]
        public required string Senha { get; set; }

        public List<NivelAcessoDto> NiveisAcesso { get; set; } = new();
    }

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
