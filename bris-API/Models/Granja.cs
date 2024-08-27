namespace bris_API.Models {
    public class Granja {
        public int Id { get; set; }
        public string? Nome_propriedade { get; set; }
        public int AgroindustriaId { get; set; }
        public Agroindustria? Agroindustria { get; set; }
        public ICollection<Animal>? Animais { get; set; }
        public string? Endereco { get; set; }
        public string? CNPJ { get; set; }
        public ICollection<Granja_Usuario_Tipo>? Granjas_Usuarios_Tipos { get; set; }
    }
}
