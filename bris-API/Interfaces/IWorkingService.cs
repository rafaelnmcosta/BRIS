using bris_API.Data;
using Microsoft.AspNetCore.Authorization;

namespace bris_API.Services
{
    public interface IWorkingService
    {
        void ConfigurePolicies(AuthorizationOptions options, AppDbContext dbContext);
    }
}