using SistemaCadastral.API.Models;

namespace SistemaCadastral.API.Services.Interfaces;

public interface ICidadeService
{
    Task<Cidade> AddAsync(Cidade cidade);
    Task<Cidade?> GetByIdAsync(int id);
    Task<IEnumerable<Cidade>> GetAllAsync();
    Task<Cidade?> GetByNomeAndEstadoAsync(string nome, string estado);
}