namespace bris_API.DTOs
{
    public class GetUsuarioDTO
    {
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string CPF { get; set; }
        public required List<GetVinculoDTO> Vinculos { get; set; } // Lista de vínculos relacionados ao usuário
    }
}
