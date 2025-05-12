using StoreAPI.Data;
using System.Threading.Tasks;

namespace StoreAPI.Services;

public class OrderService : IOrderService
{
    private readonly OrderRepository _orderRepository;
    public OrderService(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        return await _orderRepository.DeleteOrderAsync(orderId);
    }
} 