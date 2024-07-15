using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using bris_API.Data;

var builder = WebApplication.CreateBuilder(args);

// Adicionar serviços
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000") // Altere para o seu frontend
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

// Adicionar outros serviços
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin"); // Aplica a política CORS criada

app.UseAuthorization();

app.MapControllers();

app.Run();
