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
            SeedAgroindustrias(modelBuilder);
            SeedGranjas(modelBuilder);
            SeedUsers(modelBuilder);
            SeedVinculos(modelBuilder);
            SeedPolicies(modelBuilder);
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

        public void SeedAgroindustrias(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Agroindustria>().HasData(
                new Agroindustria { Id = 1, NomeFantasia = "Agroindustria Default", RazaoSocial = "Agroindustria Default", CNPJ = "00000000000100", Ativo = true },
                new Agroindustria { Id = 2, NomeFantasia = "Agroindustria Nova", RazaoSocial = "Agroindustria Nova", CNPJ = "11111111000111", Ativo = true }
            );
        }

        public void SeedGranjas(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Granja>().HasData(
                // Granjas para Agroindustria 1
                new Granja { Id = 1, NomePropriedade = "Granja Teste 1", AgroindustriaId = 1, Endereco = "Rua teste 1", CNPJ = "99999999000199", Ativo = true },
                new Granja { Id = 2, NomePropriedade = "Granja Teste 2", AgroindustriaId = 1, Endereco = "Rua teste 2", CNPJ = "99999999000122", Ativo = true },
                // Granjas para Agroindustria 2
                new Granja { Id = 3, NomePropriedade = "Granja Nova 1", AgroindustriaId = 2, Endereco = "Rua nova 1", CNPJ = "88888888000133", Ativo = true },
                new Granja { Id = 4, NomePropriedade = "Granja Nova 2", AgroindustriaId = 2, Endereco = "Rua nova 2", CNPJ = "88888888000144", Ativo = true }
            );
        }

        public void SeedUsers(ModelBuilder modelBuilder)
        {
            // Gerar hash e salt para a senha "123456"
            var salt = _passwordService.GenerateSalt();
            var hash = _passwordService.HashPassword("123456", salt);

            // Adiciona 5 usuários com roles correspondentes
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario { Id = 1, Nome = "Admin", Email = "admin@gmail.com", CPF = "00000000000", DataCadastro = DateTime.UtcNow },
                new Usuario { Id = 2, Nome = "Gestor Granja", Email = "gestor_granja@gmail.com", CPF = "11111111111", DataCadastro = DateTime.UtcNow },
                new Usuario { Id = 3, Nome = "Gestor Agro", Email = "gestor_agro@gmail.com", CPF = "22222222222", DataCadastro = DateTime.UtcNow },
                new Usuario { Id = 4, Nome = "Tecnico", Email = "tecnico@gmail.com", CPF = "33333333333", DataCadastro = DateTime.UtcNow },
                new Usuario { Id = 5, Nome = "Visualizador", Email = "visualizador@gmail.com", CPF = "44444444444", DataCadastro = DateTime.UtcNow }
            );

            // Adiciona as senhas para os usuários
            modelBuilder.Entity<Senha>().HasData(
                new Senha { Id = 1, UsuarioId = 1, SenhaHash = hash, Salt = salt },
                new Senha { Id = 2, UsuarioId = 2, SenhaHash = hash, Salt = salt },
                new Senha { Id = 3, UsuarioId = 3, SenhaHash = hash, Salt = salt },
                new Senha { Id = 4, UsuarioId = 4, SenhaHash = hash, Salt = salt },
                new Senha { Id = 5, UsuarioId = 5, SenhaHash = hash, Salt = salt }
            );
        }

        public void SeedVinculos(ModelBuilder modelBuilder)
        {
            // Cria um vínculo para cada usuário, associando a role correspondente
            modelBuilder.Entity<Vinculo>().HasData(
                // Admin (sem granja/agro)
                new Vinculo { 
                    Id = 1, 
                    UsuarioId = 1, 
                    RoleId = 1,
                    GranjaId = null,
                    AgroindustriaId = null
                },
                
                // Gestor de Granja
                new Vinculo { 
                    Id = 2, 
                    UsuarioId = 2, 
                    RoleId = 2,
                    GranjaId = 1,
                    AgroindustriaId = 1
                },
                
                // Gestor de Agroindústria
                new Vinculo { 
                    Id = 3, 
                    UsuarioId = 3, 
                    RoleId = 3,
                    GranjaId = null,
                    AgroindustriaId = 1
                },

                // Tecnico
                new Vinculo { 
                    Id = 4, 
                    UsuarioId = 4, 
                    RoleId = 4,
                    GranjaId = 1,
                    AgroindustriaId = 1
                },

                // Visualizador
                new Vinculo { 
                    Id = 5, 
                    UsuarioId = 5, 
                    RoleId = 5,
                    GranjaId = 1,
                    AgroindustriaId = 1
                }
            );
        }

        public void SeedPolicies(ModelBuilder modelBuilder)
        {
            // Dicionário de policies
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
                    // Mapeia o nome da role para seu ID
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

            modelBuilder.Entity<PolicyRole>().HasData(policyRoles);
        }
    }
}
