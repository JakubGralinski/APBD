## Key Concepts

### Models
Models are C# classes that represent the data structure of your application. They define the shape of your data and the relationships between different entities. In Entity Framework Core, models are used to create database tables and define the schema. Each model typically corresponds to a table in your database.

### DTOs (Data Transfer Objects)
DTOs are objects that define how data will be sent over the network. They help to:
- Control what data is exposed to the client
- Reduce the amount of data transferred
- Separate your API's internal data structure from its external representation
- Prevent over-posting attacks

### Database Context
The Database Context is the primary class that coordinates Entity Framework functionality for a given data model. It:
- Manages database connections
- Tracks changes to entities
- Handles database queries
- Manages relationships between entities
- Applies database migrations

### Repositories
Repositories are classes that handle data access logic. They:
- Abstract the data access layer
- Provide a collection-like interface for accessing domain objects
- Encapsulate the logic required to access data sources
- Centralize common data access functionality
- Make the code more maintainable and testable

### Services
Services contain the business logic of your application. They:
- Implement complex business rules
- Coordinate between different repositories
- Handle transactions
- Transform data between models and DTOs
- Implement validation logic

### Controllers
Controllers handle HTTP requests and responses. They:
- Receive HTTP requests
- Call appropriate services
- Return HTTP responses
- Handle routing
- Manage request/response formatting

---

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

Models are the foundation of your application's data structure. They define:
- Properties that map to database columns
- Relationships between entities
- Data validation rules
- Business rules specific to the entity

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

DTOs help you control what data is exposed to the client. They are particularly useful for:
- Hiding sensitive data
- Reducing payload size
- Shaping data for specific use cases
- Preventing over-posting attacks

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

The Database Context is your application's gateway to the database. It:
- Manages database connections
- Tracks changes to entities
- Handles database queries
- Manages relationships between entities
- Applies database migrations

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

Repositories encapsulate the logic required to access data sources. They:
- Provide a clean separation of concerns
- Make the code more maintainable
- Make the code more testable
- Centralize data access logic
- Handle database operations

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

Services implement the business logic of your application. They:
- Coordinate between different repositories
- Implement complex business rules
- Handle data transformation
- Implement validation logic
- Manage transactions

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

Controllers are the entry points for HTTP requests. They:
- Handle routing
- Process incoming requests
- Call appropriate services
- Format responses
- Handle HTTP status codes

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

Dependency Injection (DI) is a design pattern that:
- Reduces coupling between components
- Makes the code more testable
- Improves maintainability
- Enables better separation of concerns
- Makes the application more modular

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

The database connection string contains all the information needed to connect to your database:
- Server name
- Database name
- Authentication method
- Security settings
- Connection timeouts

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

Entity Framework Core migrations:
- Create the database schema
- Track changes to your data model
- Enable version control for your database
- Allow for database updates
- Support multiple database providers

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

Swagger (OpenAPI) provides:
- Interactive API documentation
- API testing interface
- Request/response examples
- Schema documentation
- Authentication testing

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

The .gitignore file:
- Excludes unnecessary files from version control
- Prevents sensitive data from being committed
- Reduces repository size
- Improves collaboration
- Maintains clean version control

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

API testing is crucial for:
- Verifying functionality
- Ensuring reliability
- Testing edge cases
- Validating responses
- Checking error handling

You can test the API using tools like Postman or curl:

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

Proper error handling:
- Provides meaningful error messages
- Maintains security
- Improves debugging
- Enhances user experience
- Prevents application crashes

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

Logging helps with:
- Debugging issues
- Monitoring application health
- Tracking user activity
- Auditing
- Performance monitoring

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

---

## 18. Additional Resources

These resources will help you:
- Learn more about .NET
- Understand best practices
- Find solutions to common problems
- Stay updated with new features
- Improve your development skills

- [.NET Documentation](https://docs.microsoft.com/en-us/dotnet/)
- [Entity Framework Core](https://docs.microsoft.com/en-us/ef/core/)
- [ASP.NET Core Web API](https://docs.microsoft.com/en-us/aspnet/core/web-api/)
- [Swagger/OpenAPI](https://swagger.io/docs/)
- [SQL Server](https://docs.microsoft.com/en-us/sql/sql-server/)
- [C# Programming Guide](https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/)
- [ASP.NET Core Security](https://docs.microsoft.com/en-us/aspnet/core/security/)
- [Entity Framework Core Performance](https://docs.microsoft.com/en-us/ef/core/performance/)
- [ASP.NET Core Middleware](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/)
- [Dependency Injection in ASP.NET Core](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/dependency-injection)

