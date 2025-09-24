using SistemaCadastral.API.Models;

namespace SistemaCadastral.API.Repositories.Interfaces;

public interface IPessoaRepository
{
    Task<Pessoa> AddAsync(Pessoa pessoa);
    Task<Pessoa?> GetByIdAsync(int id);
    Task<IEnumerable<Pessoa>> GetAllAsync();
    Task<Pessoa?> GetByCpfCnpjAsync(string cpfCnpj);
    Task UpdateAsync(Pessoa pessoa);
    Task DeleteAsync(int id);
}