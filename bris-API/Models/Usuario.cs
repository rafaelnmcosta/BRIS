namespace bris_API.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public int TipoUsuarioId { get; set; }
        public TipoUsuario? TipoUsuario { get; set; }
        public Senha? Senha { get; set; }
        public ICollection<UsuarioPermissao>? UsuarioPermissoes { get; set; }
    }
}
