using Microsoft.EntityFrameworkCore;

namespace bris_API.Services
{
    public interface IPopulateDbService
    {
        void SeedData(ModelBuilder modelBuilder);
    }
}
