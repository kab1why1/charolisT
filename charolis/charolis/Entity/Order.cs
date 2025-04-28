using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.EntityFrameworkCore;

namespace charolis.Entity;

public class Order : BaseEntity
{
    public override int Id { get; set; }
    [Required(ErrorMessage = "Користувач обов'язковий")]
    public int UserId { get; set; }
    [ValidateNever]
    public User User { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    [Range(0, double.MaxValue)]
    public decimal Total { get; set; }
    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
}