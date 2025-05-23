using System.ComponentModel.DataAnnotations;

namespace WarehouseAPI.Models
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
    }
} 