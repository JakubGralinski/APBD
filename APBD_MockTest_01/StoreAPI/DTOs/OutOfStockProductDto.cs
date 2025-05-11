namespace StoreAPI.DTOs;

public class OutOfStockProductDto
{
    public string ProductName { get; set; }
    public string Description { get; set; }
    public string LastInStockDate { get; set; } // string for flexible date formatting
} 