using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

using bris_API.Data;
using bris_API.Services;

var builder = WebApplication.CreateBuilder(args);

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

// Configuração do DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configuração do serviço de token
builder.Services.AddScoped<ITokenService, TokenService>();

// Configuração da Autenticação JWT
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
});

//Configuração da Autorização
builder.Services.AddAuthorization(options =>
{
    //Define as roles que pertencem a cada política de acesso
    var VisualizaTotal = new[] { "ADMIN" };
    var VisualizaAgro = new[] { "ADMIN", "GESTOR_AGRO", "VISUALIZADOR" };
    var VisualizaGranja = new[] { "ADMIN", "GESTOR_AGRO", "GESTOR_GRANJA", "VISUALIZADOR" };
    var VisualizaAnimais = new[] { "ADMIN", "GESTOR_AGRO", "GESTOR_GRANJA", "VISUALIZADOR", "TECNICO" };
    var GerenciaTotal = new[] { "ADMIN" };
    var GerenciaAgro = new[] { "ADMIN", "GESTOR_AGRO" };
    var GerenciaGranja = new[] { "ADMIN", "GESTOR_GRANJA" };
    var GerenciaAnimais = new[] { "ADMIN", "GESTOR_GRANJA", "TECNICO" };
    var TodosUsuarios = new[] { "ADMIN", "GESTOR_AGRO", "GESTOR_GRANJA", "VISUALIZADOR", "TECNICO" };

    //Cria as policies com a lista dos roles correspondentes
    options.AddPolicy("VisualizaTotal", policy => policy.RequireRole(VisualizaTotal));
    options.AddPolicy("VisualizaAgro", policy => policy.RequireRole(VisualizaAgro));
    options.AddPolicy("VisualizaGranja", policy => policy.RequireRole(VisualizaGranja));
    options.AddPolicy("VisualizaAnimais", policy => policy.RequireRole(VisualizaAnimais));
    options.AddPolicy("GerenciaTotal", policy => policy.RequireRole(GerenciaTotal));
    options.AddPolicy("GerenciaAgro", policy => policy.RequireRole(GerenciaAgro));
    options.AddPolicy("GerenciaGranja", policy => policy.RequireRole(GerenciaGranja));
    options.AddPolicy("GerenciaAnimais", policy => policy.RequireRole(GerenciaAnimais));
    options.AddPolicy("TodosUsuarios", policy => policy.RequireRole(TodosUsuarios));
});

// Adicionando os serviços e configurações restantes
builder.Services.AddControllers();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BRIS API", Version = "v1.1" });

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
builder.Logging.AddConsole();


var app = builder.Build();

// Configuração do Swagger
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        {
            c.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
        }
    );
}

app.UseCors("AllowSpecificOrigin");

// Usar autenticação e autorização
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
