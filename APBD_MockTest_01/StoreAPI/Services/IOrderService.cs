using System.Threading.Tasks;

namespace StoreAPI.Services;

public interface IOrderService
{
    Task<bool> DeleteOrderAsync(int orderId);
} 