using Microsoft.EntityFrameworkCore;
using StoreAPI.Models;

namespace StoreAPI.Data;

public class OrderRepository
{
    private readonly StoreContext _context;
    public OrderRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            var orderItems = _context.OrderItems.Where(oi => oi.OrderId == orderId);
            _context.OrderItems.RemoveRange(orderItems);
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
                return false;
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }
} 