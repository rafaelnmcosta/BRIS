namespace bris_API.Models
{
    public class Permissao
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public ICollection<UsuarioPermissao> UsuarioPermissoes { get; set; }
    }
}
