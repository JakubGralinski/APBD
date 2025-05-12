
## 1. Initialize the Project

```bash
dotnet new webapi -n StoreAPI
cd StoreAPI
```

---

## 2. Add Required NuGet Packages

```bash
dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Swashbuckle.AspNetCore
```

---

## 3. Create Folders

Create the following folder structure:
```
StoreAPI/
├── Models/
├── DTOs/
├── Data/
├── Services/
└── Controllers/
```

---

## 4. Create Models

In the `Models` folder, create the following classes:

### Product.cs
```csharp
public class Product
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string Description { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
```

### OrderItem.cs
```csharp
public class OrderItem
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public int OrderId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public Product Product { get; set; }
    public Order Order { get; set; }
}
```

### Order.cs
```csharp
public class Order
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public Customer Customer { get; set; }
    public ICollection<OrderItem> OrderItems { get; set; }
}
```

### Customer.cs
```csharp
public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public ICollection<Order> Orders { get; set; }
}
```

---

## 5. Create DTOs

In the `DTOs` folder, create:

### OutOfStockProductDto.cs
```csharp
public class OutOfStockProductDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}
```

---

## 6. Configure the Database Context

In the `Data` folder, create `StoreContext.cs`:

```csharp
public class StoreContext : DbContext
{
    public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<Customer> Customers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure relationships
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);
    }
}
```

---

## 7. Create Repositories

In the `Data` folder, create:

### ProductRepository.cs
```csharp
public class ProductRepository
{
    private readonly StoreContext _context;

    public ProductRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Product>> GetOutOfStockProducts()
    {
        return await _context.Products
            .Where(p => p.StockQuantity == 0)
            .ToListAsync();
    }

    // Add other product-related methods
}
```

### OrderRepository.cs
```csharp
public class OrderRepository
{
    private readonly StoreContext _context;

    public OrderRepository(StoreContext context)
    {
        _context = context;
    }

    public async Task<bool> DeleteOrder(int orderId)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return false;

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return true;
    }

    // Add other order-related methods
}
```

---

## 8. Create Services

In the `Services` folder, create:

### IProductService.cs
```csharp
public interface IProductService
{
    Task<IEnumerable<OutOfStockProductDto>> GetOutOfStockProducts();
}
```

### ProductService.cs
```csharp
public class ProductService : IProductService
{
    private readonly ProductRepository _productRepository;

    public ProductService(ProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<IEnumerable<OutOfStockProductDto>> GetOutOfStockProducts()
    {
        var products = await _productRepository.GetOutOfStockProducts();
        return products.Select(p => new OutOfStockProductDto
        {
            Id = p.Id,
            Name = p.Name,
            Price = p.Price,
            StockQuantity = p.StockQuantity
        });
    }
}
```

### IOrderService.cs
```csharp
public interface IOrderService
{
    Task<bool> DeleteOrder(int orderId);
}
```

### OrderService.cs
```csharp
public class OrderService : IOrderService
{
    private readonly OrderRepository _orderRepository;

    public OrderService(OrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<bool> DeleteOrder(int orderId)
    {
        return await _orderRepository.DeleteOrder(orderId);
    }
}
```

---

## 9. Implement the Controller

In the `Controllers` folder, create `StoreController.cs`:

```csharp
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

    [HttpGet("out-of-stock")]
    public async Task<ActionResult<IEnumerable<OutOfStockProductDto>>> GetOutOfStockProducts()
    {
        var products = await _productService.GetOutOfStockProducts();
        return Ok(products);
    }

    [HttpDelete("order/{orderId}")]
    public async Task<IActionResult> DeleteOrder(int orderId)
    {
        var result = await _orderService.DeleteOrder(orderId);
        if (!result) return NotFound();
        return NoContent();
    }
}
```

---

## 10. Configure Dependency Injection

In `Program.cs`, add:

```csharp
var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();

// Register services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// Add controllers and Swagger
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
```

---

## 11. Configure Database Connection

In `appsettings.json`, add:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=StoreDB;Trusted_Connection=True;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

---

## 12. Create the Database

Run the following commands:

```bash
# Install EF Core tools if not already installed
dotnet tool install --global dotnet-ef

# Create initial migration
dotnet ef migrations add InitialCreate

# Apply migration to database
dotnet ef database update

# Optional: Generate SQL script
dotnet ef migrations script -o script.sql
```

---

## 13. Test with Swagger

1. Run the application:
```bash
dotnet run
```

2. Open your browser and navigate to:
   - Swagger UI: `https://localhost:5001/swagger`
   - API endpoints:
     - GET `/api/store/out-of-stock`
     - DELETE `/api/store/order/{orderId}`

---

## 14. Add .gitignore

Create a `.gitignore` file with:

```
# .NET Core
bin/
obj/
*.user
*.userosscache
*.suo
*.userprefs

# Visual Studio
.vs/
.vscode/
*.swp
*.*~
project.lock.json
.DS_Store
*.pyc
nupkg/

# SQL Server files
*.mdf
*.ldf
*.ndf

# User-specific files
*.rsuser
*.suo
*.user
*.userosscache
*.sln.docstates

# Build results
[Dd]ebug/
[Dd]ebugPublic/
[Rr]elease/
[Rr]eleases/
x64/
x86/
[Aa][Rr][Mm]/
[Aa][Rr][Mm]64/
bld/
[Bb]in/
[Oo]bj/
[Ll]og/
[Ll]ogs/
```

---

## 15. Testing the API


### Get Out-of-Stock Products
```bash
curl -X GET "https://localhost:5001/api/store/out-of-stock" -H "accept: application/json"
```

### Delete an Order
```bash
curl -X DELETE "https://localhost:5001/api/store/order/1" -H "accept: application/json"
```

---

## 16. Error Handling

Add global exception handling in `Program.cs`:

```csharp
app.UseExceptionHandler(errorApp =>
{
    errorApp.Run(async context =>
    {
        context.Response.StatusCode = 500;
        context.Response.ContentType = "application/json";
        
        var error = context.Features.Get<IExceptionHandlerFeature>();
        if (error != null)
        {
            await context.Response.WriteAsJsonAsync(new
            {
                StatusCode = 500,
                Message = "An error occurred while processing your request."
            });
        }
    });
});
```

---

## 17. Logging

Add logging configuration in `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Information"
    }
  }
}
```


## 18. Additional Resources

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/)
- [Swagger/OpenAPI](https://swagger.io/docs/)
- [SQL Server](https://docs.microsoft.com/en-us/sql/sql-server/)

