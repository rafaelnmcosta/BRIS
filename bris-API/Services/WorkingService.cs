using bris_API.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;


namespace bris_API.Services
{
    public class WorkingService : IWorkingService
    {
        private readonly AppDbContext _context;
    
        public WorkingService(AppDbContext context)
        {
            _context = context;
        }

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
            
            options.AddPolicy("AcessoLoginPolicy", policy => policy.RequireClaim("AcessoLogin", "true"));
        }

        public async Task<bool> ValidaUsuario(string token)
        {
            // Extrai informações do token
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return false;

            // Obtém os valores das claims no token
            var vinculoIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "vinculoId")?.Value;
            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            var granjaIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "granjaId")?.Value;
            var agroindustriaIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "agroindustriaId")?.Value;

            // Valida se as claims foram não nulas foram extraídas corretamente
            if (string.IsNullOrEmpty(vinculoIdClaim) || string.IsNullOrEmpty(roleClaim))
                return false;

            // Valida se os IDs são valores numéricos válidos
            if (!int.TryParse(vinculoIdClaim, out int vinculoId) ||
                (granjaIdClaim != null && !int.TryParse(granjaIdClaim, out int granjaId)) ||
                (agroindustriaIdClaim != null && !int.TryParse(agroindustriaIdClaim, out int agroindustriaId)))
                return false;

            // Verifica se o vínculo ainda existe no banco e se as informações coincidem
            var vinculo = await _context.Vinculos
                .Include(v => v.Role)
                .FirstOrDefaultAsync(v => v.Id == vinculoId);

            // pega os valores de id de granja e 
            granjaId = int.Parse(granjaIdClaim);
            agroindustriaId = int.Parse(agroindustriaIdClaim);

            if (vinculo == null || vinculo.Role.Nome != roleClaim || // se a role for null ou diferente
                (granjaIdClaim != null && vinculo.GranjaId != granjaId) || // se a granja (não sendo null) for diferente
                (agroindustriaIdClaim != null && vinculo.AgroindustriaId != agroindustriaId)) // verifica agroindustria (não sendo null)
            {
                return false;
            }

            return true;
        }
    }
}