namespace bris_API.Models
{
    public class Animal
    {
        public int Id { get; set; }
        public string? Info { get; set; }
        public ICollection<Amostra>? Amostras { get; set; }
        public ResultadoFinal? ResultadoFinal { get; set; }
    }
}
