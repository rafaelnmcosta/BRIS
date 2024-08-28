namespace bris_API.Models {
    public class Agroindustria {
        public int Id { get; set; }
        public string? NomeFantasia { get; set; }
        public string? RazaoSocial { get; set; }
        public string? CNPJ { get; set; }
        public ICollection<Granja>? Granjas { get; set; }
        public ICollection<Usuario>? Usuarios { get; set; }
        public bool Ativo { get; set; }
    }    
}
