namespace bris_API.Models {
    public class Vinculo {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public int? GranjaId { get; set; }
        public Granja? Granja { get; set; }
        public int? AgroindustriaId { get; set; }
        public Agroindustria? Agroindustria { get; set; }
    }
}
