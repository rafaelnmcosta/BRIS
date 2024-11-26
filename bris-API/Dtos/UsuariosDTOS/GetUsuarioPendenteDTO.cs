namespace bris_API.DTOs
{
    public class GetUsuarioPendenteDTO
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string CPF { get; set; }
    }
}
