using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetoAPI.Models;

[Table("usuarios")]
public class User
{
    [Key]
    [Column("id_usuario")]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    [MaxLength(150)]
    [Column("email")]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MaxLength(255)]
    [Column("senha")]
    public string Password { get; set; } = string.Empty;

    [Required]
    [MaxLength(20)]
    [Column("role")]
    public string Role { get; set; } = "cliente";
}
