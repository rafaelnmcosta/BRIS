namespace bris_API.Models
{
    public class Semana
    {
        public int Id { get; set; }
        public int NSemana { get; set; }
        public ICollection<Amostra>? Amostras { get; set; }
    }
}
