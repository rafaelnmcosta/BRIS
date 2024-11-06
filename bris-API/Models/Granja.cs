using System.Text.Json.Serialization;

namespace bris_API.Models {
    public class Granja {
        public int Id { get; set; }
        public string? NomePropriedade { get; set; }
        public int AgroindustriaId { get; set; }
        public Agroindustria? Agroindustria { get; set; }
        public ICollection<Animal>? Animais { get; set; }
        public string? Endereco { get; set; }
        public string? CNPJ { get; set; }

        [JsonIgnore]
        public ICollection<Vinculo>? Vinculos { get; set; }
        public bool Ativo { get; set; }
    }
}
