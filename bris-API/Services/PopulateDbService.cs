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
            SeedTiposUsuario(modelBuilder);
            SeedAgroindustria(modelBuilder);
            SeedGranja(modelBuilder);
            SeedAdmin(modelBuilder);
            SeedVinculos(modelBuilder);
        }

        private void SeedTiposUsuario(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TipoUsuario>().HasData(
                new TipoUsuario { Id = 1, Tipo = "ADMIN", Descricao = "Administrador do sistema" },
                new TipoUsuario { Id = 2, Tipo = "GESTOR_GRANJA", Descricao = "Gestor de granjas" },
                new TipoUsuario { Id = 3, Tipo = "GESTOR_AGRO", Descricao = "Gestor de agroindústrias" },
                new TipoUsuario { Id = 4, Tipo = "TECNICO", Descricao = "Técnico de campo" },
                new TipoUsuario { Id = 5, Tipo = "VISUALIZADOR", Descricao = "Usuário com acesso somente leitura" },
                new TipoUsuario { Id = 98, Tipo = "PENDENTE", Descricao = "Usuário pendente de ativação" },
                new TipoUsuario { Id = 99, Tipo = "INATIVO", Descricao = "Usuário inativo" }
            );
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
            modelBuilder.Entity<Vinculos>().HasData(
                new Vinculos { Id = 1, UsuarioId = 1, GranjaId = 1, TipoUsuarioId = 1 }
            );
        }
    }
}
