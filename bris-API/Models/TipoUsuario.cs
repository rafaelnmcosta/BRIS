namespace bris_API.Models
{
    public class TipoUsuario
    {
        public int Id { get; set; }
        public string? Tipo { get; set; }
        public string? Descricao { get; set; }
        public ICollection<GranjaUsuarioTipo>? GranjasUsuariosTipos { get; set; }
    }
}