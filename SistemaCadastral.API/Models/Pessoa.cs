using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SistemaCadastral.API.Models;

public class Pessoa
{
    [Key]
    public int ID { get; set; }

    [Required]
    [StringLength(70)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(2)]
    public string TipoPessoa { get; set; } = string.Empty; // JU ou FI

    [Required]
    [StringLength(14)]
    public string CpfCnpj { get; set; } = string.Empty;

    [Required]
    [StringLength(8)]
    public string Cep { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string Endereco { get; set; } = string.Empty;

    [Required]
    public int Numero { get; set; }

    [StringLength(20)]
    public string Compl { get; set; } = string.Empty;

    [Required]
    [StringLength(50)]
    public string Bairro { get; set; } = string.Empty;

    [Required]
    public DateTime DataNascimentoFundacao { get; set; }

    [Required]
    public DateTime DataCadastro { get; set; } = DateTime.Now;

    [Required]
    [StringLength(50)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Celular { get; set; } = string.Empty;

    [Required]
    [StringLength(2)]
    public string SitCadastral { get; set; } = "A"; // A ou I

    [ForeignKey("Cidade")]
    public int CidadeID { get; set; }

    public Cidade? Cidade { get; set; }
}