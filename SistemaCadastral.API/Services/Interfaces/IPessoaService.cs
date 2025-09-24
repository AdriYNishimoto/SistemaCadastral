using SistemaCadastral.API.Models;

namespace SistemaCadastral.API.Services.Interfaces;

public interface IPessoaService
{
    Task<Pessoa> RegistrarAsync(Pessoa pessoa);
    Task<IEnumerable<Pessoa>> ConsultarAsync(string? tipoPessoa, string? cidadeNome, string? estado);
    Task<IEnumerable<object>> GerarRelatorioBasicoAsync();
    Task<IEnumerable<Pessoa>> GerarRelatorioCompletoAsync();
}