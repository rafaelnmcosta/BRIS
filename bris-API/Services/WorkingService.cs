using bris_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace bris_API.Services
{
    public class WorkingService : IWorkingService
    {
        public void ConfigurePolicies(AuthorizationOptions options, AppDbContext dbContext)
        {
            var policies = dbContext.Policy.Include(p => p.PolicyRoles).ThenInclude(pr => pr.Role).ToList();

            foreach (var policy in policies)
            {
                Console.WriteLine("\n------------------------- POLICY RETIRADA DO BANCO DE DADOS:\n" + policy);
                options.AddPolicy(policy.Nome, policyBuilder =>
                {
                    var roleNames = policy.PolicyRoles.Select(pr => pr.Role.Nome).ToArray();
                    policyBuilder.RequireRole(roleNames);
                });
            }
        }
    }
}