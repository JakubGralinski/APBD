using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace StoreAPI.Models;

public class Product
{
    [Key]
    public int ProductId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Decimal Price { get; set; }
    public int StockQuantity { get; set; }
}