namespace bris_API.Models
{
    public class Semana
    {
        public int Id { get; set; }
        public int NroSemana { get; set; }
        public int Resultado { get; set; }
        /* Valores para resultado:
            0: Menor;
            1: Medio;
            2: Maior;
        */
        public int AvaliacaoId { get; set; }
        public Avaliacao? Avaliacao {get; set; }
        public ICollection<Dose>? Doses { get; set; }
    }
}
