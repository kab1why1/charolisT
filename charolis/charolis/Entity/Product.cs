using System.ComponentModel.DataAnnotations;

namespace charolis.Entity;

public class Product : BaseEntity
{
    override public int Id { get; set; }
    [Required]
    [MaxLength(100)]
    public string Name { get; set; }
    [StringLength(200)]
    public string? Description { get; set; }
    [Required]
    [Range(01, 1000)]
    public decimal Price { get; set; }
    public bool IsActive { get; set; } =  true;
}