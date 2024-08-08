namespace bris_API.Models {
    public class Granja_Usuario_Tipo {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public int GranjaId { get; set; }
        public Granja? Granja { get; set; }
        public int TipoUsuarioId { get; set; }
        public TipoUsuario? TipoUsuario { get; set; }
    }
}
