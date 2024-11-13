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
            var acessoLogin = jwtToken.Claims.FirstOrDefault(c => c.Type == "AcessoLogin")?.Value;
            var vinculoIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            // Verifica se o token é de um usuário que acabou de fazer login (pula a validação)
            if (string.IsNullOrEmpty(acessoLogin))
                return true;

            // Valida se o ID do vinculo é um valor numérico válido
            if (!int.TryParse(vinculoIdClaim, out int vinculoId))
                return false;

            // Verifica se o vínculo ainda existe no banco e se as informações coincidem
            var vinculo = await _context.Vinculos
                .Include(v => v.Role)
                .FirstOrDefaultAsync(v => v.Id == vinculoId);

            if (vinculo == null)
            {
                return false;
            }

            return true;
        }
    }
}