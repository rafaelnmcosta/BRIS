using System.Text.Json.Serialization;

namespace bris_API.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public ICollection<Vinculo>? Vinculos { get; set; }
        public ICollection<PolicyRole>? PolicyRoles { get; set; }
    }
}