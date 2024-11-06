namespace bris_API.Models {
    public class PolicyRole {
        public int Id { get; set; }
        public int RoleId { get; set; }
        public Role? Role { get; set; }
        public int PolicyId { get; set; }
        public Policy? Policy { get; set; }
    }
}
