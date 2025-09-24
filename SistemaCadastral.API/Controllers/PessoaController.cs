using Microsoft.AspNetCore.Mvc;
using SistemaCadastral.API.Models;
using SistemaCadastral.API.Services.Interfaces;

namespace SistemaCadastral.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PessoaController : ControllerBase
{
    private readonly IPessoaService _pessoaService;

    public PessoaController(IPessoaService pessoaService)
    {
        _pessoaService = pessoaService;
    }

    [HttpPost("registrar")]
    public async Task<ActionResult<Pessoa>> Registrar([FromBody] Pessoa pessoa)
    {
        try
        {
            var registrada = await _pessoaService.RegistrarAsync(pessoa);
            return Ok(registrada);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Erro interno: " + ex.Message);
        }
    }

    [HttpGet("consultar")]
    public async Task<ActionResult<IEnumerable<Pessoa>>> Consultar(string? tipoPessoa = "Todos", string? cidade = null, string? estado = null)
    {
        var pessoas = await _pessoaService.ConsultarAsync(tipoPessoa, cidade, estado);
        return Ok(pessoas.Select(p => new
        {
            p.ID,
            p.Nome,
            p.CpfCnpj,
            p.TipoPessoa,
            Cidade = p.Cidade.Nome,
            Estado = p.Cidade.Estado,
            p.DataCadastro,
            Idade = CalcularIdade(p.DataNascimentoFundacao) 
        }));
    }

    [HttpGet("relatorio/basico")]
    public async Task<ActionResult<IEnumerable<object>>> RelatorioBasico()
    {
        return Ok(await _pessoaService.GerarRelatorioBasicoAsync());
    }

    [HttpGet("relatorio/completo")]
    public async Task<ActionResult<IEnumerable<Pessoa>>> RelatorioCompleto()
    {
        return Ok(await _pessoaService.GerarRelatorioCompletoAsync());
    }

    private int CalcularIdade(DateTime dataNasc)
    {
        var idade = DateTime.Now.Year - dataNasc.Year;
        if (DateTime.Now.DayOfYear < dataNasc.DayOfYear) idade--;
        return idade;
    }
}