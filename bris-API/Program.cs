using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Logging.Console;
using Microsoft.OpenApi.Models;
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
            builder.WithOrigins(
                "https://frontend-bris:443",  // Serviço do proxy reverso no docker-compose
                "https://200.137.215.116",
		        "https://localhost:443",
                "http://localhost:3000",
                "http://localhost:5000"
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
        });
});


// Configuração da Autenticação Jwt
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    // Verifica se tudo foi preenchido corretamente no arquivo de appsettings.json
    if (
        string.IsNullOrEmpty(builder.Configuration["Jwt:Issuer"]) ||
        string.IsNullOrEmpty(builder.Configuration["Jwt:Audience"]) ||
        string.IsNullOrEmpty(builder.Configuration["Jwt:Key"]) ||
        string.IsNullOrEmpty(builder.Configuration["Jwt:ExpiresInMinutes"]) ||
        string.IsNullOrEmpty(builder.Configuration["Jwt:RenewInMinutesLeft"])
    )
    {
        throw new Exception("Configurações de Jwt ausentes ou inválidas.");
    }

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
            var token = context.Request.Cookies["auth_token"];

            if (string.IsNullOrEmpty(token))
            {
                Console.WriteLine("Token não encontrado no cookie.");
            }
            else
            {
                Console.WriteLine($"Token capturado: {token}");
                context.Token = token;
            }

            return Task.CompletedTask;
        },


        OnTokenValidated = async context =>
        {
            var tokenService = context.HttpContext.RequestServices.GetRequiredService<ITokenService>();

            // Valida os dados do token
            await tokenService.ValidaContext(context);
            // Se ValidaContext não falhou, renova o token se necessário
            if (context.Result?.Succeeded != false) 
            {
                tokenService.RenovaToken(context);
            }
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    // Acessa o banco de dados para pegar as policies registradas e adicioná-las à aplicação
    using (var scope = builder.Services.BuildServiceProvider().CreateScope())
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        var policies = dbContext.Policy.Include(p => p.PolicyRoles).ThenInclude(pr => pr.Role).ToList();

        foreach (var policy in policies)
        {
            options.AddPolicy(policy.Nome, policyBuilder =>
            {
                var roleNames = policy.PolicyRoles.Select(pr => pr.Role.Nome).ToArray();
                policyBuilder.RequireRole(roleNames);
            });
        }
    }

    // Adicionando a política específica de AcessoLogin
    options.AddPolicy("AcessoLoginPolicy", policy => policy.RequireClaim("AcessoLogin", "true"));


    // Policy de operação OU entre AcessoLogin e TodosUsuarios (para as listas de vinculos) 
    options.AddPolicy("AcessoLoginOuTodosUsuarios", policy =>
    {
        policy.RequireAssertion(context =>
            // Verifica se o token é de login
            context.User.HasClaim(c => c.Type == "AcessoLogin" && c.Value == "true")
            ||
            // Se não é de login, verifica se o token tem qualquer valor na claim "role"
            context.User.HasClaim(c => c.Type == ClaimTypes.Role && !string.IsNullOrEmpty(c.Value))
        );
    });
});



// Adicionando o swagger
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BRIS API", Version = "v1.1" });
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
app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1"); });

app.MapControllers();

app.Run();
