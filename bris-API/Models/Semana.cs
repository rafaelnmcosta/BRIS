using System.Text.Json.Serialization;

namespace bris_API.Models
{
    public class Semana
    {
        public int Id { get; set; }
        public int NroSemana { get; set; }
        public int Resultado { get; set; }
        /* Valores para resultado da Sensibilidade ao Resfriamento:
            1: Menor;
            2: Medio;
            3: Maior;
            0: Erro;
            -1: Não gerado;
        */
        public int AvaliacaoId { get; set; }
        public Avaliacao? Avaliacao {get; set; }

        [JsonIgnore]
        public ICollection<Dose>? Doses { get; set; }
    }
}
