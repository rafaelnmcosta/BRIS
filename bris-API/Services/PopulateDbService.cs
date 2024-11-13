using bris_API.Models;
using Microsoft.EntityFrameworkCore;

namespace bris_API.Services
{
    public class PopulateDbService : IPopulateDbService
    {
        private readonly IPasswordService _passwordService;

        public PopulateDbService(IPasswordService passwordService)
        {
            _passwordService = passwordService;
        }

        public void SeedData(ModelBuilder modelBuilder)
        {
            SeedRoles(modelBuilder);
            SeedAgroindustria(modelBuilder);
            SeedGranja(modelBuilder);
            SeedAdmin(modelBuilder);
            SeedVinculos(modelBuilder);
        }

        public void SeedRoles(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role { Id = 1, Nome = "ADMIN", Descricao = "Administrador do sistema" },
                new Role { Id = 2, Nome = "GESTOR_GRANJA", Descricao = "Gestor de granjas" },
                new Role { Id = 3, Nome = "GESTOR_AGRO", Descricao = "Gestor de agroindústrias" },
                new Role { Id = 4, Nome = "TECNICO", Descricao = "Técnico da granja" },
                new Role { Id = 5, Nome = "VISUALIZADOR", Descricao = "Usuário com acesso somente de visualização ao sistema" },
                new Role { Id = 98, Nome = "PENDENTE", Descricao = "Usuário pendente de ativação" },
                new Role { Id = 99, Nome = "INATIVO", Descricao = "Usuário inativo" }
            );
        }

        public void SeedPolicies(ModelBuilder modelBuilder)
        {
            // alterar nesse dicionario se quiser mais alguma policy incluída no seed de dados
            var policies = new Dictionary<string, string[]>
            {
                { "VisualizaTotal", new[] { "ADMIN" } },
                { "VisualizaAgroindustria", new[] { "ADMIN", "GESTOR_AGRO", "VISUALIZADOR" } },
                { "VisualizaUsuarios", new[] { "ADMIN", "GESTOR_AGRO", "GESTOR_GRANJA", "VISUALIZADOR" } },
                { "VisualizaAnimais", new[] { "ADMIN", "GESTOR_AGRO", "GESTOR_GRANJA", "VISUALIZADOR", "TECNICO" } },
                { "GerenciaTotal", new[] { "ADMIN" } },
                { "GerenciaAgroindustria", new[] { "ADMIN", "GESTOR_AGRO" } },
                { "GerenciaUsuarios", new[] { "ADMIN", "GESTOR_AGRO", "GESTOR_GRANJA" } },
                { "GerenciaAnimais", new[] { "ADMIN", "GESTOR_GRANJA", "TECNICO" } },
                { "TodosUsuarios", new[] { "ADMIN", "GESTOR_AGRO", "GESTOR_GRANJA", "VISUALIZADOR", "TECNICO" } }
            };

            int policyId = 1;
            var policyRoles = new List<PolicyRole>();

            foreach (var policy in policies)
            {
                modelBuilder.Entity<Policy>().HasData(
                    new Policy { Id = policyId, Nome = policy.Key, Descricao = $"Policy para {policy.Key}" }
                );

                foreach (var roleName in policy.Value)
                {
                    // Mapeia o role para a ID correta
                    int roleId = roleName switch
                    {
                        "ADMIN" => 1,
                        "GESTOR_GRANJA" => 2,
                        "GESTOR_AGRO" => 3,
                        "TECNICO" => 4,
                        "VISUALIZADOR" => 5,
                        _ => throw new Exception($"Role não reconhecida: {roleName}")
                    };

                    policyRoles.Add(new PolicyRole { Id = policyId * 10 + roleId, PolicyId = policyId, RoleId = roleId });
                }

                policyId++;
            }

            // Insere as relações PolicyRole
            modelBuilder.Entity<PolicyRole>().HasData(policyRoles);
        }

        private void SeedAgroindustria(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agroindustria>().HasData(
                new Agroindustria { Id = 1, NomeFantasia = "Agroindustria Default", RazaoSocial = "Agroindustria Default", CNPJ = "00000000000100", Ativo = true }
            );
        }

        private void SeedGranja(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Granja>().HasData(
                new Granja { Id = 1, NomePropriedade = "Granja Teste", AgroindustriaId = 1, Endereco = "Rua teste", CNPJ = "99999999000199", Ativo = true }
            );
        }

        private void SeedAdmin(ModelBuilder modelBuilder)
        {
            var salt = _passwordService.GenerateSalt();
            var hash = _passwordService.HashPassword("123456", salt);

            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nome = "Admin", Email = "admin@gmail.com", CPF = "00000000000" }
            );

            modelBuilder.Entity<Senha>().HasData(
                new Senha { Id = 1, UsuarioId = 1, SenhaHash = hash, Salt = salt }
            );
        }

        private void SeedVinculos(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Vinculo>().HasData(
                new Vinculo { Id = 1, UsuarioId = 1, GranjaId = 1, RoleId = 1 }
            );
        }
    }
}
