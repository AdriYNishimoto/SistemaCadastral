using Microsoft.EntityFrameworkCore;
using SistemaCadastral.API.Models;

namespace SistemaCadastral.API.Data;

public class AppDbContext : DbContext
{
    public DbSet<Pessoa> Pessoas { get; set; }
    public DbSet<Cidade> Cidades { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Pessoa>()
            .HasIndex(p => p.CpfCnpj)
            .IsUnique();

        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Cidade>().HasData(new Cidade { ID = 1, Nome = "São Paulo", Estado = "SP" });
    }
}