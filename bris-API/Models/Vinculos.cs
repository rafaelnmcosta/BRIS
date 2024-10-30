namespace bris_API.Models {
    public class Vinculos {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int TipoUsuarioId { get; set; }
        public TipoUsuario? TipoUsuario { get; set; }
        public int? GranjaId { get; set; }
        public Granja? Granja { get; set; }
        public int? AgroindustriaId { get; set; }
        public Agroindustria? Agroindustria { get; set; }
    }
}
