namespace bris_API.Models {
    public class Agroindustria {
        public int Id { get; set; }
        public string? NomeFantasia { get; set; }
        public string? RazaoSocial { get; set; }
        public string? CNPJAgroindustria { get; set; }
        public ICollection<Granja>? Granjas { get; set; }
    }    
}
