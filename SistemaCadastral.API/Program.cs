using Microsoft.EntityFrameworkCore;
using SistemaCadastral.API.Data;
using SistemaCadastral.API.Repositories;
using SistemaCadastral.API.Repositories.Interfaces;
using SistemaCadastral.API.Services;
using SistemaCadastral.API.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Adicionar DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Adicionar serviços e repositórios
builder.Services.AddScoped<IPessoaService, PessoaService>();
builder.Services.AddScoped<ICidadeService, CidadeService>();
builder.Services.AddScoped<IPessoaRepository, PessoaRepository>();
builder.Services.AddScoped<ICidadeRepository, CidadeRepository>();

// Adicionar controllers
builder.Services.AddControllers();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});

builder.WebHost.UseUrls("http://localhost:5288");

var app = builder.Build();

app.UseCors();
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();

app.Run();