using System.Text.RegularExpressions;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using SistemaCadastral.API.Models;
using SistemaCadastral.API.Repositories.Interfaces;
using SistemaCadastral.API.Services.Interfaces;

namespace SistemaCadastral.API.Services;

public class PessoaService : IPessoaService
{
    private readonly IPessoaRepository _pessoaRepository;
    private readonly ICidadeRepository _cidadeRepository;

    public PessoaService(IPessoaRepository pessoaRepository, ICidadeRepository cidadeRepository)
    {
        _pessoaRepository = pessoaRepository;
        _cidadeRepository = cidadeRepository;
    }

    public async Task<Pessoa> RegistrarAsync(Pessoa pessoa)
    {
        // Validar email (reforçando DataAnnotations)
        if (!IsValidEmail(pessoa.Email))
            throw new ArgumentException("Email inválido.");

        // Validar CPF/CNPJ
        if (pessoa.TipoPessoa == "FI" && !IsValidCpf(pessoa.CpfCnpj))
            throw new ArgumentException("CPF inválido.");
        if (pessoa.TipoPessoa == "JU" && !IsValidCnpj(pessoa.CpfCnpj))
            throw new ArgumentException("CNPJ inválido.");

        // Checar unicidade de CPF/CNPJ
        var existente = await _pessoaRepository.GetByCpfCnpjAsync(pessoa.CpfCnpj);
        if (existente != null)
            throw new ArgumentException("CPF/CNPJ já cadastrado.");

        // Cidade: Se não existe, cadastrar
        if (pessoa.Cidade == null)
            throw new ArgumentException("Cidade não informada.");

        var cidade = await _cidadeRepository.GetByNomeAndEstadoAsync(pessoa.Cidade.Nome, pessoa.Cidade.Estado);
        if (cidade == null)
        {
            cidade = await _cidadeRepository.AddAsync(pessoa.Cidade);
        }
        pessoa.CidadeID = cidade.ID;
        pessoa.Cidade = null; // Evitar ciclo de serialização

        return await _pessoaRepository.AddAsync(pessoa);
    }

    public async Task<IEnumerable<Pessoa>> ConsultarAsync(string? tipoPessoa, string? cidadeNome, string? estado)
    {
        var query = await _pessoaRepository.GetAllAsync();

        // Filtrar por tipo de pessoa
        if (!string.IsNullOrEmpty(tipoPessoa) && tipoPessoa != "Todos")
            query = query.Where(p => p.TipoPessoa == tipoPessoa).ToList();

        // Filtrar por cidade
        if (!string.IsNullOrEmpty(cidadeNome))
            query = query.Where(p => p.Cidade != null && p.Cidade.Nome.ToLower() == cidadeNome.ToLower()).ToList();

        // Filtrar por estado
        if (!string.IsNullOrEmpty(estado))
            query = query.Where(p => p.Cidade != null && p.Cidade.Estado.ToLower() == estado.ToLower()).ToList();

        return query;
    }

    public async Task<IEnumerable<object>> GerarRelatorioBasicoAsync()
    {
        var pessoas = await _pessoaRepository.GetAllAsync();
        return pessoas.Select(p => new { p.CpfCnpj, p.Celular }).ToList();
    }

    public async Task<IEnumerable<Pessoa>> GerarRelatorioCompletoAsync()
    {
        return await _pessoaRepository.GetAllAsync();
    }

    // Métodos de validação
    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        var regex = new Regex(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$");
        return regex.IsMatch(email);
    }

    private bool IsValidCpf(string cpf)
    {
        // Remover máscaras (pontos, traços, etc.)
        cpf = cpf.Replace(".", "").Replace("-", "").Trim();
        if (cpf.Length != 11 || !cpf.All(char.IsDigit))
            return false;

        // Verificar se todos os dígitos são iguais (ex.: 11111111111)
        if (cpf.Distinct().Count() == 1)
            return false;

        // Cálculo do primeiro dígito verificador
        int[] multiplicadores1 = { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCpf = cpf.Substring(0, 9);
        int soma = 0;
        for (int i = 0; i < 9; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores1[i];
        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        // Cálculo do segundo dígito verificador
        int[] multiplicadores2 = { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
        tempCpf += digito1;
        soma = 0;
        for (int i = 0; i < 10; i++)
            soma += int.Parse(tempCpf[i].ToString()) * multiplicadores2[i];
        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        // Verificar se os dígitos calculados correspondem aos informados
        return cpf.EndsWith($"{digito1}{digito2}");
    }

    private bool IsValidCnpj(string cnpj)
    {
        // Remover máscaras (pontos, traços, barras, etc.)
        cnpj = cnpj.Replace(".", "").Replace("-", "").Replace("/", "").Trim();
        if (cnpj.Length != 14 || !cnpj.All(char.IsDigit))
            return false;

        // Verificar se todos os dígitos são iguais (ex.: 00000000000000)
        if (cnpj.Distinct().Count() == 1)
            return false;

        // Cálculo do primeiro dígito verificador
        int[] multiplicadores1 = { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        string tempCnpj = cnpj.Substring(0, 12);
        int soma = 0;
        for (int i = 0; i < 12; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores1[i];
        int resto = soma % 11;
        int digito1 = resto < 2 ? 0 : 11 - resto;

        // Cálculo do segundo dígito verificador
        int[] multiplicadores2 = { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
        tempCnpj += digito1;
        soma = 0;
        for (int i = 0; i < 13; i++)
            soma += int.Parse(tempCnpj[i].ToString()) * multiplicadores2[i];
        resto = soma % 11;
        int digito2 = resto < 2 ? 0 : 11 - resto;

        // Verificar se os dígitos calculados correspondem aos informados
        return cnpj.EndsWith($"{digito1}{digito2}");
    }
}