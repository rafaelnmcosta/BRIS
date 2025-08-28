using System.ComponentModel.DataAnnotations;

namespace bris_API.DTOs
{
    public class GetAvaliacaoDTO
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public string Linhagem { get; set; } = string.Empty; // vem do Animal
        public DateTime DataInicioAvaliacao { get; set; }
        public int StatusAvaliacao { get; set; }
        public bool? ResultadoFinal { get; set; }
    }
}