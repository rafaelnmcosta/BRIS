using System.Text.Json.Serialization;

namespace bris_API.Models
{
    public class Role
    {
        public int Id { get; set; }
        /* Valores padrões de inserção no banco (vide PopulateDbService por via de duvidas):
            1: ADMIN
            2: GESTOR_AGRO
            3: GESTOR_GRANJA
            4: TECNICO
            5: VISUALIZADOR
            98: PENDENTE
            99: INATIVO
        */
        public string? Nome { get; set; }
        public string? Descricao { get; set; }
        public ICollection<Vinculo>? Vinculos { get; set; }
        public ICollection<PolicyRole>? PolicyRoles { get; set; }
    }
}