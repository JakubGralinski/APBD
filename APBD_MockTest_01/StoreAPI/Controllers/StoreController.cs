using Microsoft.AspNetCore.Mvc;
using StoreAPI.Services;
using StoreAPI.DTOs;

namespace StoreAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StoreController : ControllerBase
{
    private readonly IProductService _productService;
    private readonly IOrderService _orderService;

    public StoreController(IProductService productService, IOrderService orderService)
    {
        _productService = productService;
        _orderService = orderService;
    }

    // GET: api/store/out-of-stock?sortBy=name|date
    [HttpGet("out-of-stock")]
    public async Task<ActionResult<IEnumerable<OutOfStockProductDto>>> GetOutOfStockProducts([FromQuery] string sortBy = "name")
    {
        var products = await _productService.GetOutOfStockProductsAsync(sortBy);
        return Ok(products);
    }

    // DELETE: api/store/order/{orderId}
    [HttpDelete("order/{orderId}")]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        var result = await _orderService.DeleteOrderAsync(orderId);
        if (!result)
            return NotFound(new { message = "Order not found." });
        return NoContent();
    }
}