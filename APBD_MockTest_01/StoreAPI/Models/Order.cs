using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StoreAPI.Models;

public class Order
{
    [Key]
    public int OrderId { get; set; }
    [ForeignKey(nameof(Customer))]
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public Decimal TotalAmount { get; set; }
}