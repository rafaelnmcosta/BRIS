using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging.Console;
using System.Text;

using bris_API.Data;
using bris_API.Services;

var builder = WebApplication.CreateBuilder(args);

// configuração do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// injeção de dependência dos serviços
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IPasswordService, PasswordService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPopulateDbService, PopulateDbService>();
builder.Services.AddScoped<IResultsService, ResultsService>();

// adicionando os controllers
builder.Services.AddControllers();

// Configuração do CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Configuração da Autenticação JWT com validação de IP e User-Agent
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
    };

    // Intercepta o token antes da validação para descriptografá-lo
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var encryptedToken = context.Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (!string.IsNullOrEmpty(encryptedToken))
            {
                try
                {
                    var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
                    var decryptedToken = tokenService.DecryptToken(encryptedToken);

                    // Substitui o token descriptografado no contexto para a validação
                    context.Token = decryptedToken;
                }
                catch (Exception ex)
                {
                    // Adiciona um log de erro se a descriptografia falhar
                    var logger = context.HttpContext.RequestServices.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Falha ao descriptografar o token. Token inválido!");
                    context.Fail("Token inválido!");
                }
            }
            return Task.CompletedTask;
        },
        OnTokenValidated = context =>
        {
            var currentIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var currentUserAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();

            // Recupera as claims de IP e User-Agent do token já validado
            var tokenIpClaim = context.Principal.FindFirst("UserIP")?.Value;
            var tokenUserAgentClaim = context.Principal.FindFirst("UserAgent")?.Value;

            // Compara as claims do token com os valores atuais
            if (tokenIpClaim != currentIp || tokenUserAgentClaim != currentUserAgent)
            {
                context.Fail("IP ou navegador não correspondentes com a geração do token.");
            }

            return Task.CompletedTask;
        }
    };

});

//Configuração dos níveis de Autorização
var policyRolesMap = new Dictionary<string, string[]>
{
    { "VisualizaTotal", new[] { "ADMIN" } },
    { "VisualizaAgro", new[] { "ADMIN", "GESTOR_AGRO", "VISUALIZADOR" } },
    { "VisualizaGranja", new[] { "ADMIN", "GESTOR_AGRO", "GESTOR_GRANJA", "VISUALIZADOR" } },
    { "VisualizaAnimais", new[] { "ADMIN", "GESTOR_AGRO", "GESTOR_GRANJA", "VISUALIZADOR", "TECNICO" } },
    { "GerenciaTotal", new[] { "ADMIN" } },
    { "GerenciaAgro", new[] { "ADMIN", "GESTOR_AGRO" } },
    { "GerenciaGranja", new[] { "ADMIN", "GESTOR_GRANJA" } },
    { "GerenciaAnimais", new[] { "ADMIN", "GESTOR_GRANJA", "TECNICO" } },
    { "TodosUsuarios", new[] { "ADMIN", "GESTOR_AGRO", "GESTOR_GRANJA", "VISUALIZADOR", "TECNICO" } }
};

// Configuração de Autorização baseada nas Roles do dicionário
builder.Services.AddAuthorization(options =>
{
    foreach (var policy in policyRolesMap)
    {
        options.AddPolicy(policy.Key, policyBuilder =>
            policyBuilder.RequireRole(policy.Value));
    }
});

// Adicionando o swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BRIS API", Version = "v1.1" });

    // adicionando o token no swagger
    c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Por favor, insira o token JWT no formato: Bearer {token}",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Configuração de Logging para o Console
builder.Logging.ClearProviders();
builder.Logging.AddConsole(options =>
{
    if (builder.Environment.IsDevelopment())
    {
        options.FormatterName = ConsoleFormatterNames.Simple; // Formato simples para desenvolvimento
        options.IncludeScopes = true; // Inclui o escopo para facilitar a depuração em dev
    }
    else if (builder.Environment.IsProduction())
    {
        options.FormatterName = ConsoleFormatterNames.Systemd; // Formato systemd para produção
    }
});
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging")); // configura o logging a partir do appsettings.json



var app = builder.Build();

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c =>{c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");});

app.MapControllers();

app.Run();
