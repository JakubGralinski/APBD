using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.Models;

public class OrderItem
{
    [Key]
    public int OrderItemId { get; set; }
    [ForeignKey(nameof(Order))]
    public int OrderId { get; set; }
    [ForeignKey(nameof(Product))]
    public int ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    
    public virtual Order Order { get; set; }
    public virtual Product Product { get; set; }
}