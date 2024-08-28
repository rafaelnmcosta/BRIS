namespace bris_API.Models
{
    public class Avaliacao
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public Animal? Animal { get; set; }
        public DateTime DataInicioAvaliacao { get; set; }
        public int StatusAvaliacao { get; set; }
        public bool ResultadoFinal { get; set; }
        public ICollection<Semana>? Semanas { get; set; }
    }
}
