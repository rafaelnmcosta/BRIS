namespace bris_API.Models
{
    public class ResultadoFinal
    {
        public int Id { get; set; }
        public int PorcoId { get; set; }
        public Porco Porco { get; set; }
        public bool Aprovado { get; set; }
    }
}
