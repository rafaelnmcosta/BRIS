namespace bris_API.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }
        public Senha? Senha { get; set; }
        public Agroindustria? Agroindustria { get; set; }
        public required int AgroindustriaId { get; set; }
        public ICollection<Dose>? Doses { get; set; }
        public ICollection<Animal>? Animais { get; set; }
        public ICollection<GranjaUsuarioTipo>? GranjasUsuariosTipos { get; set; }
    }
}
