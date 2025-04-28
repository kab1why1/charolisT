using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace charolis.Entity;

public class OrderItem : BaseEntity
{
    public override int Id { get; set; }
    [Required]
    public int OrderId { get; set; }
    [ValidateNever]
    public Order Order { get; set; }
    
    [Required(ErrorMessage = "Товар обов'язковий")]
    public int ProductId { get; set; }
    [ValidateNever]
    public Product Product { get; set; }
    
    [Range(1, int.MaxValue, ErrorMessage = "Кількість має бути щонайменше 1")]
    public int Quantity { get; set; }
    
    [Range(0.01, double.MaxValue, ErrorMessage = "Ціна має бути додатною")]
    public decimal PriceAtPurchase  { get; set; }
}