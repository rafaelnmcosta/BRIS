namespace bris_API.Models
{
    public class Amostra
    {
        public int Id { get; set; }
        public int NSemana { get; set; }
        public string? Resultado { get; set; }
        public int PorcoId { get; set; }
        public Porco Porco { get; set; }
        public int SemanaId { get; set; }
        public Semana Semana { get; set; }
    }
}
