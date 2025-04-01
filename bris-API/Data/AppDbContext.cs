using Microsoft.EntityFrameworkCore;
using bris_API.Models;
using bris_API.Services;

namespace bris_API.Data
{
    public class AppDbContext : DbContext
    {
        
        private readonly IPopulateDbService _populateDbService;

        public AppDbContext(DbContextOptions<AppDbContext> options, IPopulateDbService populateDbService) 
            : base(options)
        {
            _populateDbService = populateDbService;
        }

        public DbSet<Agroindustria> Agroindustrias { get; set; }
        public DbSet<Granja> Granjas { get; set; }
        public DbSet<Animal> Animais { get; set; }
        public DbSet<Avaliacao> Avaliacoes { get; set; }
        public DbSet<Semana> Semanas { get; set; }
        public DbSet<Dose> Doses { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Senha> Senhas { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Policy> Policy { get; set; }
        public DbSet<PolicyRole> PolicyRoles { get; set; }
        public DbSet<Vinculo> Vinculos { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Relação 1-1 Usuario-Senha
            modelBuilder.Entity<Usuario>()
                .HasOne(u => u.Senha)
                .WithOne(s => s.Usuario)
                .HasForeignKey<Senha>(s => s.UsuarioId);
            
            // Relação 1-N Agroindustria-Granja
            modelBuilder.Entity<Granja>()
                .HasOne(g => g.Agroindustria)
                .WithMany(a => a.Granjas)
                .HasForeignKey(g => g.AgroindustriaId);

            // Relação 1-N Granja-Animal
            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Granja)
                .WithMany(g => g.Animais)
                .HasForeignKey(a => a.GranjaId);

            // Relação 1-N Usuario-Animal
            modelBuilder.Entity<Animal>()
                .HasOne(a => a.Usuario)
                .WithMany(u => u.Animais)
                .HasForeignKey(a => a.UsuarioResponsavelId);

            // Relação 1-N Animal-Avaliacao
            modelBuilder.Entity<Avaliacao>()
                .HasOne(av => av.Animal)
                .WithMany(a => a.Avaliacoes)
                .HasForeignKey(av => av.AnimalId);

            // Relação 1-N Avaliacao-Semana
            modelBuilder.Entity<Semana>()
                .HasOne(s => s.Avaliacao)
                .WithMany(av => av.Semanas)
                .HasForeignKey(s => s.AvaliacaoId);

            // Relação 1-N Semana-Dose
            modelBuilder.Entity<Dose>()
                .HasOne(d => d.Semana)
                .WithMany(s => s.Doses)
                .HasForeignKey(d => d.SemanaId);

            // Relação 1-N Usuario-Dose
            modelBuilder.Entity<Dose>()
                .HasOne(d => d.Usuario)
                .WithMany(u => u.Doses)
                .HasForeignKey(d => d.UsuarioId);

            // Relação N-N Vinculos
            modelBuilder.Entity<Vinculo>()
                .HasOne(v => v.Granja)
                .WithMany(g => g.Vinculos)
                .HasForeignKey(v => v.GranjaId);

            modelBuilder.Entity<Vinculo>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.Vinculos)
                .HasForeignKey(v => v.UsuarioId);

            modelBuilder.Entity<Vinculo>()
                .HasOne(v => v.Role)
                .WithMany(t => t.Vinculos)
                .HasForeignKey(v => v.RoleId);

            modelBuilder.Entity<Vinculo>()
                .HasOne(v => v.Agroindustria)
                .WithMany(a => a.Vinculos)
                .HasForeignKey(v => v.AgroindustriaId);
            
            // Relação N-N Policy-Roles
            modelBuilder.Entity<PolicyRole>()
                .HasOne(pr => pr.Policy)
                .WithMany(p => p.PolicyRoles)
                .HasForeignKey(pr => pr.PolicyId);

            modelBuilder.Entity<PolicyRole>()
                .HasOne(pr => pr.Role)
                .WithMany(r => r.PolicyRoles)
                .HasForeignKey(pr => pr.RoleId);

            // Insere informações diretamente no banco de dados
            _populateDbService.SeedData(modelBuilder);

            base.OnModelCreating(modelBuilder);
        }
    }
}
