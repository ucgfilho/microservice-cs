using System.ComponentModel.DataAnnotations;

namespace projetoAPI.DTOs;

public class RegisterDTO
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = string.Empty;

    [Required]
    [RegularExpression("^(cliente|vendedor)$", ErrorMessage = "O tipo de usuário deve ser 'cliente' ou 'vendedor'.")]
    public string Role { get; set; } = "cliente";
}
