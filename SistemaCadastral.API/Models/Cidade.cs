using System.ComponentModel.DataAnnotations;

namespace SistemaCadastral.API.Models;

public class Cidade
{
    [Key]
    public int ID { get; set; }

    [Required]
    [StringLength(25)]
    public string Nome { get; set; } = string.Empty;

    [Required]
    [StringLength(20)]
    public string Estado { get; set; } = string.Empty;
}