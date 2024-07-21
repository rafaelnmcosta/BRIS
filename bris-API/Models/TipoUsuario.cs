namespace bris_API.Models
{
    public class TipoUsuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public ICollection<Usuario>? Usuarios { get; set; }
    }
}