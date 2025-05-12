using StoreAPI.Data;
using StoreAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace StoreAPI.Services;

public class ProductService : IProductService
{
    private readonly ProductRepository _productRepository;
    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<List<OutOfStockProductDto>> GetOutOfStockProductsAsync(string sortBy = "name")
    {
        return await _productRepository.GetOutOfStockProductsAsync(sortBy);
    }
} 