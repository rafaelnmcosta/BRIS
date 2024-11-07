namespace bris_API.DTOs
{
    public class GetVinculoDTO
    {
        public required int UsuarioId { get; set; } // id do usuario
        public required int VinculoId { get; set; } // id do vinculo na tabela de vinculos
        public required string Role { get; set; } // role referida no vinculo
        public required int? GranjaId { get; set; } // id e nome da granja referida no vinculo (pode ser null)
        public string? NomeGranja { get; set; }
        public int? AgroindustriaId { get; set; } // id e nome da agroindustria referida (pode ser null)
        public string? NomeAgroindustria { get; set; }
    }
}