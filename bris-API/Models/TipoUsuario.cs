using System.Text.Json.Serialization;

namespace bris_API.Models
{
    public class TipoUsuario
    {
        public int Id { get; set; }
        public string? Tipo { get; set; }
        public string? Descricao { get; set; }

        [JsonIgnore]
        public ICollection<GranjaUsuarioTipo>? GranjasUsuariosTipos { get; set; }
    }
}