using System.Text.Json.Serialization;

namespace bris_API.Models {
    public class Agroindustria {
        public int Id { get; set; }
        public string? NomeFantasia { get; set; }
        public string? RazaoSocial { get; set; }
        public string? CNPJ { get; set; }

        [JsonIgnore]
        public ICollection<Granja>? Granjas { get; set; }

        [JsonIgnore]
        public ICollection<Usuario>? Usuarios { get; set; }

        [JsonIgnore]
        public ICollection<Vinculos>? Vinculos { get; set; }
        public bool Ativo { get; set; }
    }    
}
