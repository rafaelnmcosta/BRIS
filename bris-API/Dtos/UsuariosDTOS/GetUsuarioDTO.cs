namespace bris_API.DTOs
{
    public class GetUsuarioDTO
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataCadastro { get; set; }
        public DateTime? UltimoLogin { get; set; }
        public List<GetVinculoDTO> Vinculos { get; set; }
    }
}