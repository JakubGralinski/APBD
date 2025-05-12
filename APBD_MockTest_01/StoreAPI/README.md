# StoreAPI – Step-by-Step Setup Guide

This guide will help you build a .NET Web API for an e-commerce store, including database setup, models, repositories, services, controllers, and Swagger documentation.

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

- `Models`
- `DTOs`
- `Data`
- `Services`
- `Controllers` (already exists)

---

## 4. Create Models

In the `Models` folder, create the following classes:
- `Product.cs`
- `OrderItem.cs`
- `Order.cs`
- `Customer.cs`

Define properties according to your database schema.

---

## 5. Create DTOs

In the `DTOs` folder, create:
- `OutOfStockProductDto.cs`

This will be used for the out-of-stock products endpoint.

---

## 6. Configure the Database Context

In the `Data` folder, create:
- `StoreContext.cs`

This should inherit from `DbContext` and include `DbSet` properties for all your models.

---

## 7. Create Repositories

In the `Data` folder, create:
- `ProductRepository.cs` (handles product-related DB operations)
- `OrderRepository.cs` (handles order-related DB operations)

---

## 8. Create Services

In the `Services` folder, create:
- `IProductService.cs` (interface for product business logic)
- `ProductService.cs` (implementation, uses `ProductRepository`)
- `IOrderService.cs` (interface for order business logic)
- `OrderService.cs` (implementation, uses `OrderRepository`)

---

## 9. Implement the Controller

In the `Controllers` folder, update or create:
- `StoreController.cs`

Inject the services and implement endpoints for:
- Retrieving out-of-stock products (`GET /api/store/out-of-stock`)
- Deleting an order (`DELETE /api/store/order/{orderId}`)

---

## 10. Configure Dependency Injection

In `Program.cs`, register all services and repositories:

```csharp
builder.Services.AddDbContext<StoreContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<ProductRepository>();
builder.Services.AddScoped<OrderRepository>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddControllers();
```

---

## 11. Enable Controller Routing

In `Program.cs`, before `app.Run();`, add:

```csharp
app.MapControllers();
```

---

## 12. Configure Database Connection

In `appsettings.json`, add your SQL Server connection string:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=StoreDB;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

---

## 13. Create the Database

Generate and apply migrations:

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

To generate a SQL script:
```bash
dotnet ef migrations script -o script.sql
```

---

## 14. Test with Swagger

Run your project:

```bash
dotnet run
```

Open the Swagger UI in your browser (usually at `https://localhost:5001/swagger`) to test your endpoints.

---

## 15. Add .gitignore

Create a `.gitignore` file in the root directory and include standard .NET, IDE, and OS-specific ignores.

---

## Done!

You now have a clean, layered .NET Web API project with:
- Models, DTOs, Repositories, Services, Controllers
- Dependency injection
- Database migrations
- Swagger documentation

---

**Feel free to use this as your project README!** 