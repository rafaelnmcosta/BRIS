using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class GetPerfilDTO
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string CPF { get; set; }
        public required string Role { get; set; }
        public  string? NomeAgroindustria { get; set; }

        public string? NomeGranja { get; set; }   
    }
}