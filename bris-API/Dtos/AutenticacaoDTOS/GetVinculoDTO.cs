namespace bris_API.DTOs
{
    public class GetVinculoDTO
    {
        public required int Id { get; set; }
        public required string Role { get; set; } // role referida no vinculo
        public string? NomeGranja { get; set; }
        public string? NomeAgroindustria { get; set; }
    }
}