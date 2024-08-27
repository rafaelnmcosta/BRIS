namespace bris_API.Models
{
    public class TipoUsuario
    {
        public int Id { get; set; }
        public string? Tipo { get; set; }
        public string? Descricao { get; set; }
        public ICollection<Granja_Usuario_Tipo>? Granjas_Usuarios_Tipos { get; set; }
    }
}