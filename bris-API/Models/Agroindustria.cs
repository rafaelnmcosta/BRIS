namespace bris_API.Models
{
    public class Agroindustria
    {
        public int Id { get; set; }
        public string? NomeFantasia { get; set; }
        public string? RazaoSocial { get; set; }
        public string? CNPJ { get; set; }
        public string? Endereco { get; set; }
        public string? Email { get; set; }
        public string? Telefone { get; set; }
        public ICollection<Granja>? Granjas { get; set; }
        public ICollection<Vinculo>? Vinculos { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public bool Ativo { get; set; }
    }
}
