using SistemaCadastral.API.Models;

namespace SistemaCadastral.API.Repositories.Interfaces;

public interface ICidadeRepository
{
    Task<Cidade> AddAsync(Cidade cidade);
    Task<Cidade?> GetByIdAsync(int id);
    Task<IEnumerable<Cidade>> GetAllAsync();
    Task<Cidade?> GetByNomeAndEstadoAsync(string nome, string estado);
}