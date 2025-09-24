using System;
using System.Threading.Tasks;
using SistemaCadastral.API.Models;
using SistemaCadastral.API.Repositories.Interfaces;
using SistemaCadastral.API.Services.Interfaces;

namespace SistemaCadastral.API.Services;

public class CidadeService : ICidadeService
{
    private readonly ICidadeRepository _cidadeRepository;

    public CidadeService(ICidadeRepository cidadeRepository)
    {
        _cidadeRepository = cidadeRepository;
    }

    public async Task<Cidade> AddAsync(Cidade cidade)
    {
        // Validações
        if (cidade == null)
            throw new ArgumentNullException(nameof(cidade), "Cidade não pode ser nula.");

        if (string.IsNullOrWhiteSpace(cidade.Nome) || cidade.Nome.Length > 25)
            throw new ArgumentException("Nome da cidade é obrigatório e deve ter no máximo 25 caracteres.");

        if (string.IsNullOrWhiteSpace(cidade.Estado) || cidade.Estado.Length > 20)
            throw new ArgumentException("Estado é obrigatório e deve ter no máximo 20 caracteres.");

        // Verificar se a cidade já existe (evitar duplicatas)
        var existente = await _cidadeRepository.GetByNomeAndEstadoAsync(cidade.Nome, cidade.Estado);
        if (existente != null)
            throw new ArgumentException($"Cidade {cidade.Nome}/{cidade.Estado} já está cadastrada.");

        return await _cidadeRepository.AddAsync(cidade);
    }

    public async Task<Cidade?> GetByIdAsync(int id)
    {
        if (id <= 0)
            throw new ArgumentException("ID da cidade deve ser maior que zero.");

        return await _cidadeRepository.GetByIdAsync(id);
    }

    public async Task<IEnumerable<Cidade>> GetAllAsync()
    {
        return await _cidadeRepository.GetAllAsync();
    }

    public async Task<Cidade?> GetByNomeAndEstadoAsync(string nome, string estado)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("Nome da cidade é obrigatório.");

        if (string.IsNullOrWhiteSpace(estado))
            throw new ArgumentException("Estado é obrigatório.");

        return await _cidadeRepository.GetByNomeAndEstadoAsync(nome, estado);
    }
}