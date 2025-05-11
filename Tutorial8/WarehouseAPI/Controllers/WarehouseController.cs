using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WarehouseAPI.Data;
using WarehouseAPI.DTOs;
using WarehouseAPI.Models;
using Microsoft.Data.SqlClient;

namespace WarehouseAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WarehouseController : ControllerBase
    {
        private readonly WarehouseContext _context;

        public WarehouseController(WarehouseContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> AddProductToWarehouse(ProductWarehouseRequest request)
        {
            // Check if amount is valid
            if (request.Amount <= 0)
            {
                return BadRequest("Amount must be greater than 0");
            }
            // Check if product exists
            var product = await _context.Products.FindAsync(request.IdProduct);
            if (product == null)
            {
                return NotFound("Product not found");
            }

            // Check if warehouse exists
            var warehouse = await _context.Warehouses.FindAsync(request.IdWarehouse);
            if (warehouse == null)
            {
                return NotFound("Warehouse not found");
            }

            // Check if order exists and is not fulfilled
            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.IdProduct == request.IdProduct && 
                                        o.Amount == request.Amount && 
                                        o.CreatedAt < request.CreatedAt &&
                                        o.FulfilledAt == null);

            if (order == null)
            {
                return NotFound("No matching order found");
            }

            // Check if order is already fulfilled
            var existingFulfillment = await _context.ProductWarehouses
                .AnyAsync(pw => pw.IdOrder == order.IdOrder);

            if (existingFulfillment)
            {
                return BadRequest("Order already fulfilled");
            }

            var now = DateTime.UtcNow;

            // Create new product warehouse entry
            var productWarehouse = new ProductWarehouse
            {
                IdWarehouse = request.IdWarehouse,
                IdProduct = request.IdProduct,
                IdOrder = order.IdOrder,
                Amount = request.Amount,
                Price = product.Price * request.Amount,
                CreatedAt = now
            };

            // Update order fulfillment
            order.FulfilledAt = now;

            // Save changes
            _context.ProductWarehouses.Add(productWarehouse);
            await _context.SaveChangesAsync();

            return Ok(productWarehouse.IdProductWarehouse);
        }

        [HttpPost("stored-proc")]
        public async Task<IActionResult> AddProductToWarehouseUsingProc(ProductWarehouseRequest request)
        {
            if (request.Amount <= 0)
            {
                return BadRequest("Amount must be greater than 0");
            }

            var connectionString = _context.Database.GetConnectionString();
            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand("AddProductToWarehouse", connection);
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@IdProduct", request.IdProduct);
            command.Parameters.AddWithValue("@IdWarehouse", request.IdWarehouse);
            command.Parameters.AddWithValue("@Amount", request.Amount);
            command.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);

            try
            {
                await connection.OpenAsync();
                var result = await command.ExecuteScalarAsync();
                if (result != null && int.TryParse(result.ToString(), out int newId))
                {
                    return Ok(newId);
                }
                else
                {
                    return StatusCode(500, "Failed to insert record using stored procedure.");
                }
            }
            catch (SqlException ex)
            {
                // Check for custom errors from RAISERROR
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
} 