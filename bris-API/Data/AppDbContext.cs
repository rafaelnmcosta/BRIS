using Microsoft.EntityFrameworkCore;
using bris_API.Models;

namespace bris_API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Porco> Porcos { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}
