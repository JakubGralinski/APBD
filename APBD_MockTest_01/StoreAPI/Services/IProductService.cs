using StoreAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreAPI.Services;

public interface IProductService
{
    Task<List<OutOfStockProductDto>> GetOutOfStockProductsAsync(string sortBy = "name");
} 