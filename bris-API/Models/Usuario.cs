namespace bris_API.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public int TipoUsuarioId { get; set; }
        public TipoUsuario? TipoUsuario { get; set; }
        public Senha? Senha { get; set; }
        public ICollection<Dose>? Doses { get; set; }
        public ICollection<Animal>? Animais { get; set; }
        public ICollection<Granja_Usuario_Tipo>? Granjas_Usuarios_Tipos { get; set; }
    }
}
