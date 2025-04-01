using System.Text.Json.Serialization;

namespace bris_API.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Email { get; set; }
        public string? CPF { get; set; }
        public string? Telefone { get; set; }
        public DateTime DataCadastro { get; set; } = DateTime.UtcNow;
        public DateTime? UltimoLogin { get; set; }

        [JsonIgnore]
        public Senha? Senha { get; set; }
        public ICollection<Dose>? Doses { get; set; }
        public ICollection<Animal>? Animais { get; set; }
        public ICollection<Vinculo>? Vinculos { get; set; }
    }
}
