namespace bris_API.DTOs
{

    public class GetSemanaDTO
    {
        public int Id { get; set; }
        public int NroSemana { get; set; }
        public int Resultado { get; set; }
        public ICollection<GetDoseDTO> Doses { get; set; } = new List<GetDoseDTO>();
    }
}
