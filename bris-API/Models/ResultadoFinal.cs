namespace bris_API.Models
{
    public class ResultadoFinal
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public Animal Animal { get; set; }
        public bool Aprovado { get; set; }
    }
}
