namespace bris_API.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string? Linhagem { get; set; }
        public int Idade { get; set; }
        public float Peso { get; set; }
        public bool Status { get; set; }
        public int GranjaId { get; set; }
        public Granja? Granja { get; set; }
        public int UsuarioResponsavelId { get; set; }
        public Usuario? Usuario { get; set;}
        public ICollection<Avaliacao>? Avaliacoes {get; set;}
    }
}
