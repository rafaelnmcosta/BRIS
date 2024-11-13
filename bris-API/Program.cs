using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging.Console;
using Microsoft.OpenApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using bris_API.Data;
using bris_API.Services;
using bris_API.Controllers;

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

// injeção de dependência das interfaces dos controllers
builder.Services.AddScoped<IAutenticacaoController, AutenticacaoController>();
builder.Services.AddScoped<IUsuariosController, UsuariosController>();
builder.Services.AddScoped<IPerfilController, PerfilController>();

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

    // Intercepta o token para a validação
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            // Buscando o token JWT a partir do cookie HTTP-Only
            context.Token = context.Request.Cookies["auth_token"];
            return Task.CompletedTask;
        },

        OnTokenValidated = async context =>
        {
            var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();
            
            var token = context.SecurityToken as JwtSecurityToken;
            if (token == null)
            {
                context.Fail("Token inválido!");
                return;
            }

            // Lógica para renovar o token
            var timeToExpire = token.ValidTo - DateTime.UtcNow;
            if (timeToExpire.TotalMinutes < double.Parse(builder.Configuration["Jwt:RenewInMinutesLeft"])) // expira em menos de 5 minutos
            {
                var newToken = tokenService.GenerateTokenVinculo(context.Principal.FindFirst(ClaimTypes.NameIdentifier).Value, 
                                                                context.Principal.FindFirst("UserIP").Value, 
                                                                context.Principal.FindFirst("UserAgent").Value);
                tokenService.SetCookieToken(context.HttpContext, newToken);
            }

            // Continuação das validações (consistência, IP e User-Agent)
            var isValid = await tokenService.ValidaUsuario(token.RawData);

            if (!isValid)
            {
                context.Fail("Vínculo inválido ou expirado. Faça login novamente.");
            }

            var currentIp = context.HttpContext.Connection.RemoteIpAddress?.ToString();
            var currentUserAgent = context.HttpContext.Request.Headers["User-Agent"].ToString();
            var tokenIpClaim = context.Principal.FindFirst("UserIP")?.Value;
            var tokenUserAgentClaim = context.Principal.FindFirst("UserAgent")?.Value;

            if (tokenIpClaim != currentIp || tokenUserAgentClaim != currentUserAgent)
            {
                context.Fail("IP ou navegador não correspondentes com a geração do token.");
            }
        }
    };
});

// 
builder.Services.AddAuthorization(options =>
{
    // Adicionando a política específica de AcessoLogin
    options.AddPolicy("AcessoLoginPolicy", policy => policy.RequireClaim("AcessoLogin", "true"));

    // Acessa o banco de dados para pegar as policies registradas e adicioná-las à aplicação
    using (var scope = builder.Services.BuildServiceProvider().CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

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
});



// Adicionando o swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BRIS API", Version = "v1.1" });

    // adicionando o token jwt no swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Por favor, insira o token JWT no formato: Bearer {token}",
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
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
        options.FormatterName = ConsoleFormatterNames.Simple; // formato simples para desenvolvimento
        options.IncludeScopes = true; // incluindo o escopo para facilitar a depuração em dev
    }
    else if (builder.Environment.IsProduction())
    {
        options.FormatterName = ConsoleFormatterNames.Systemd; // formato systemd para produção
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
