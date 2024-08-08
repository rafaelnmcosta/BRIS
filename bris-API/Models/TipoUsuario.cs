namespace bris_API.Models
{
    public class TipoUsuario
    {
        public int Id { get; set; }
        public string? Tipo { get; set; }

        public ICollection<Granja_Usuario_Tipo>? Granjas_Usuarios_Tipo { get; set; }
    }
}