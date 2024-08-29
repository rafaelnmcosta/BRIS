namespace bris_API.Models
{
    public class Dose
    {
        public int Id { get; set; }
        public int SemanaId { get; set; }
        public Semana? Semana { get; set; }
        public int UsuarioId { get; set; }
        public Usuario? Usuario { get; set; }
        public DateTime? DataRegistro { get; set; }
        public float? ValorRegistrado { get; set; }
        public int Ordem { get; set; }
        /* Valores para ordem:
            0: 0h;
            1: 120h;
            2: 168h;
        */
        public bool PodePreencher { get; set; }
    }
}