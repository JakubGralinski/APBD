using Microsoft.AspNetCore.Mvc;
using StoreAPI.Data;
using StoreAPI.DTOs;

namespace StoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreController : ControllerBase
{
    private readonly ProductRepository _productRepository;
    private readonly OrderRepository _orderRepository;

    public StoreController(ProductRepository productRepository, OrderRepository orderRepository)
    {
        _productRepository = productRepository;
        _orderRepository = orderRepository;
    }

    // GET: api/store/out-of-stock?sortBy=name|date
    [HttpGet("out-of-stock")]
    public async Task<ActionResult<IEnumerable<OutOfStockProductDto>>> GetOutOfStockProducts([FromQuery] string sortBy = "name")
    {
        var products = await _productRepository.GetOutOfStockProductsAsync(sortBy);
        return Ok(products);
    }

    // DELETE: api/store/order/{orderId}
    [HttpDelete("order/{orderId}")]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        var result = await _orderRepository.DeleteOrderAsync(orderId);
        if (!result)
            return NotFound(new { message = "Order not found." });
        return NoContent();
    }
}