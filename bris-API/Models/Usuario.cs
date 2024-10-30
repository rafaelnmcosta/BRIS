using System.Text.Json.Serialization;

namespace bris_API.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }
        
        [JsonIgnore]
        public Senha? Senha { get; set; }

        [JsonIgnore]
        public ICollection<Dose>? Doses { get; set; }

        [JsonIgnore]
        public ICollection<Animal>? Animais { get; set; }

        [JsonIgnore]
        public ICollection<Vinculos>? Vinculos { get; set; }
    }
}
