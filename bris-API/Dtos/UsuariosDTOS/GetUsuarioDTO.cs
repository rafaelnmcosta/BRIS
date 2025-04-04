namespace bris_API.DTOs
{
    public class GetUsuarioDTO
    {
        public int Id { get; set; }
        public required string Nome { get; set; }
        public required string Email { get; set; }
        public required string CPF { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public required List<GetVinculoDTO> Vinculos { get; set; }
    }
}