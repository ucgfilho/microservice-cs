using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace projetoAPI.Models;

[Table("produtos")]
public class Product
{
    [Key]
    [Column("id_produto")]
    public int Id { get; set; }

    [Required]
    [MaxLength(150)]
    [Column("nome")]
    public string Name { get; set; } = string.Empty;

    [MaxLength(255)]
    [Column("descricao")]
    public string? Description { get; set; }

    [Column("categoria_id")]
    public int? CategoryId { get; set; }

    [Column("preco")]
    public decimal Price { get; set; }

    [Column("estoque")]
    public int Stock { get; set; }

    [Column("ativo")]
    public bool IsActive { get; set; } = true;

    [Column("criado_em")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("atualizado_em")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("id_usuario")]
    public int UserId { get; set; }

    [ForeignKey("UserId")]
    public User? User { get; set; }
}
