using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Logging.Console;
using Microsoft.OpenApi.Models;
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
builder.Services.AddScoped<IWorkingService, WorkingService>();

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

                    // Substitui o token descriptografado no contexto para validação
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

        OnTokenValidated = async context =>
        {
            var token = context.SecurityToken as JwtSecurityToken;
            if (token == null)
            {
                context.Fail("Token inválido!");
                return;
            }

            var workingService = context.HttpContext.RequestServices.GetRequiredService<IWorkingService>();
            var isValid = await workingService.ValidaUsuario(token.RawData); // Passa o token bruto para validação

            if (!isValid)
            {
                context.Fail("Vínculo inválido ou expirado. Faça login novamente.");
            }

            // Validação de IP e User-Agent
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

// adicionando os serviços de autorização sem configuração, a configuração é feita após a declaração do app abaixo: vvvv
builder.Services.AddAuthorization();


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

// Configuração de autorização com policies dinâmicas
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var workingService = scope.ServiceProvider.GetRequiredService<IWorkingService>();

    var options = app.Services.GetRequiredService<AuthorizationOptions>();
    workingService.ConfigurePolicies(options, dbContext);
}

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.UseSwagger();
app.UseSwaggerUI(c =>{c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");});

app.MapControllers();

app.Run();
