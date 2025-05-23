using System.ComponentModel.DataAnnotations;

namespace WarehouseAPI.Models
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }
        public int IdProduct { get; set; }
        public int Amount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? FulfilledAt { get; set; }
        
        public virtual Product Product { get; set; }
    }
} 