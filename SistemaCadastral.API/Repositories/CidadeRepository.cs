using Microsoft.EntityFrameworkCore;
using SistemaCadastral.API.Data;
using SistemaCadastral.API.Models;
using SistemaCadastral.API.Repositories.Interfaces;

namespace SistemaCadastral.API.Repositories;

public class CidadeRepository : ICidadeRepository
{
    private readonly AppDbContext _context;

    public CidadeRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Cidade> AddAsync(Cidade cidade)
    {
        _context.Cidades.Add(cidade);
        await _context.SaveChangesAsync();
        return cidade;
    }

    public async Task<Cidade?> GetByIdAsync(int id)
    {
        return await _context.Cidades.FirstOrDefaultAsync(c => c.ID == id);
    }

    public async Task<IEnumerable<Cidade>> GetAllAsync()
    {
        return await _context.Cidades.ToListAsync();
    }

    public async Task<Cidade?> GetByNomeAndEstadoAsync(string nome, string estado)
    {
        return await _context.Cidades
            .FirstOrDefaultAsync(c => c.Nome.ToLower() == nome.ToLower() && c.Estado.ToLower() == estado.ToLower());
    }
}