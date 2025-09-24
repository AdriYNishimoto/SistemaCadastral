using Microsoft.AspNetCore.Mvc;
using SistemaCadastral.API.Models;
using SistemaCadastral.API.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace SistemaCadastral.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CidadeController : ControllerBase
{
    private readonly ICidadeService _cidadeService;

    public CidadeController(ICidadeService cidadeService)
    {
        _cidadeService = cidadeService;
    }

    [HttpPost]
    public async Task<ActionResult<Cidade>> Add([FromBody] Cidade cidade)
    {
        try
        {
            var novaCidade = await _cidadeService.AddAsync(cidade);
            return CreatedAtAction(nameof(GetById), new { id = novaCidade.ID }, novaCidade);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Cidade>> GetById(int id)
    {
        try
        {
            var cidade = await _cidadeService.GetByIdAsync(id);
            if (cidade == null)
                return NotFound($"Cidade com ID {id} não encontrada.");
            return Ok(cidade);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Cidade>>> GetAll()
    {
        try
        {
            var cidades = await _cidadeService.GetAllAsync();
            return Ok(cidades);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpGet("buscar")]
    public async Task<ActionResult<Cidade>> GetByNomeAndEstado([FromQuery] string nome, [FromQuery] string estado)
    {
        try
        {
            var cidade = await _cidadeService.GetByNomeAndEstadoAsync(nome, estado);
            if (cidade == null)
                return NotFound($"Cidade {nome}/{estado} não encontrada.");
            return Ok(cidade);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, [FromBody] Cidade cidade)
    {
        try
        {
            if (id <= 0 || cidade.ID != id)
                return BadRequest("ID inválido ou não corresponde ao corpo da requisição.");

            var existente = await _cidadeService.GetByIdAsync(id);
            if (existente == null)
                return NotFound($"Cidade com ID {id} não encontrada.");

            existente.Nome = cidade.Nome;
            existente.Estado = cidade.Estado;

            await _cidadeService.AddAsync(existente);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            var cidade = await _cidadeService.GetByIdAsync(id);
            if (cidade == null)
                return NotFound($"Cidade com ID {id} não encontrada.");

            await _cidadeService.AddAsync(cidade);
            return NoContent();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Erro interno: {ex.Message}");
        }
    }
}