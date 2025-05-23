using System.ComponentModel.DataAnnotations;

namespace WarehouseAPI.Models
{
    public class ProductWarehouse
    {
        [Key]
        public int IdProductWarehouse { get; set; }
        public int IdWarehouse { get; set; }
        public int IdProduct { get; set; }
        public int IdOrder { get; set; }
        public int Amount { get; set; }
        public decimal Price { get; set; }
        public DateTime CreatedAt { get; set; }
        
        public virtual Warehouse Warehouse { get; set; }
        public virtual Product Product { get; set; }
        public virtual Order Order { get; set; }
    }
} 