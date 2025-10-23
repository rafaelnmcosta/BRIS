namespace bris_API.DTOs
{
    public class GetAvaliacaoDetalhadaDTO
    {
        public int Id { get; set; }
        public int AnimalId { get; set; }
        public string Linhagem { get; set; } = string.Empty;
        public DateTime DataInicioAvaliacao { get; set; }
        public int StatusAvaliacao { get; set; }
        public string? ResultadoFinal { get; set; }

        // Semanas da avaliação
        public ICollection<GetSemanaDTO> Semanas { get; set; } = new List<GetSemanaDTO>();
    }
}
