using System.ComponentModel.DataAnnotations;

namespace projetoAPI.DTOs;

public class ProductCreateDTO
{
    [Required]
    [MaxLength(150)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    public string? Description { get; set; }

    public int? CategoryId { get; set; }

    [Range(0.0, double.MaxValue)]
    public decimal Price { get; set; }

    [Range(0, int.MaxValue)]
    public int Stock { get; set; }

    public bool IsActive { get; set; } = true;
}
