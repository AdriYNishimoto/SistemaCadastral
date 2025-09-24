using Microsoft.EntityFrameworkCore;
using SistemaCadastral.API.Data;
using SistemaCadastral.API.Models;
using SistemaCadastral.API.Repositories.Interfaces;

namespace SistemaCadastral.API.Repositories;

public class PessoaRepository : IPessoaRepository
{
    private readonly AppDbContext _context;

    public PessoaRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Pessoa> AddAsync(Pessoa pessoa)
    {
        _context.Pessoas.Add(pessoa);
        await _context.SaveChangesAsync();
        return pessoa;
    }

    public async Task<Pessoa?> GetByIdAsync(int id)
    {
        return await _context.Pessoas.Include(p => p.Cidade).FirstOrDefaultAsync(p => p.ID == id);
    }

    public async Task<IEnumerable<Pessoa>> GetAllAsync()
    {
        return await _context.Pessoas.Include(p => p.Cidade).ToListAsync();
    }

    public async Task<Pessoa?> GetByCpfCnpjAsync(string cpfCnpj)
    {
        return await _context.Pessoas.FirstOrDefaultAsync(p => p.CpfCnpj == cpfCnpj);
    }

    public async Task UpdateAsync(Pessoa pessoa)
    {
        _context.Pessoas.Update(pessoa);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var pessoa = await GetByIdAsync(id);
        if (pessoa != null)
        {
            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
        }
    }
}