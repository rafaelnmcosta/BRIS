namespace bris_API.Models
{
    public class Amostra
    {
        public int Id { get; set; }
        public int NSemana { get; set; }
        public string? Resultado { get; set; }
        public int AnimalId { get; set; }
        public Animal? Animal { get; set; }
        public int SemanaId { get; set; }
        public Semana? Semana { get; set; }
    }
}
