namespace bris_API.DTOs
{
    public class GetDoseDTO
    {
        public int Id { get; set; }
        public int SemanaId { get; set; }

        // Usuário que registrou a dose
        public int UsuarioId { get; set; }
        public GetUsuarioDTO? Usuario { get; set; }

        public DateTime? DataRegistro { get; set; }
        public float? ValorRegistrado { get; set; }

        // Ordem da dose: 0h, 120h, 168h
        public int Ordem { get; set; }

        public bool PodePreencher { get; set; }
    }
}
