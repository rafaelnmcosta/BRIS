namespace bris_API.DTOs
{
    public class GetVinculoDTO
    {
        public int Id { get; set; }
        public string Role { get; set; } // role referida no vinculo
        public int RoleId { get; set; }
        public string? NomeAgroindustria { get; set; }
        public int? AgroindustriaId { get; set; }
        public string? NomeGranja { get; set; }
        public int? GranjaId { get; set; }
    }
}