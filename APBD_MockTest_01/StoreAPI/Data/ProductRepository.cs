using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;
using StoreAPI.DTOs;

namespace StoreAPI.Data;

public class ProductRepository
{
    private readonly StoreContext _context;
    public ProductRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<List<OutOfStockProductDto>> GetOutOfStockProductsAsync(string sortBy = "name")
    {
        var products = await _context.Products
            .Where(p => p.StockQuantity == 0)
            .Select(p => new OutOfStockProductDto
            {
                ProductName = p.Name,
                Description = p.Description,
                LastInStockDate = null // will be set below
            })
            .ToListAsync();

        foreach (var product in products)
        {
            var lastOrder = await (from oi in _context.OrderItems
                                   join o in _context.Orders on oi.OrderId equals o.OrderId
                                   join p in _context.Products on oi.ProductId equals p.ProductId
                                   where p.Name == product.ProductName
                                   orderby o.OrderDate descending
                                   select o.OrderDate).FirstOrDefaultAsync();
            product.LastInStockDate = lastOrder == default ? "Never ordered" : lastOrder.ToString("yyyy-MM-dd");
        }

        if (sortBy == "date")
        {
            products = products.OrderByDescending(p => p.LastInStockDate == "Never ordered" ? null : p.LastInStockDate).ToList();
        }
        else
        {
            products = products.OrderBy(p => p.ProductName).ToList();
        }
        return products;
    }
} 