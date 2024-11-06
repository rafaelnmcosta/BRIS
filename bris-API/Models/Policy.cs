namespace bris_API.Models
{
    public class Policy
    {
        public int Id { get; set; }
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public ICollection<PolicyRole>? PolicyRoles { get; set; }
    }
}