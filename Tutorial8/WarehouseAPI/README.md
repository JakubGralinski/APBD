## Project Setup

1. Create a new Web API project:
```bash
dotnet new webapi -n WarehouseAPI
cd WarehouseAPI
```

2. Add required NuGet packages:
```bash
dotnet add package Microsoft.EntityFrameworkCore.SqlServer
dotnet add package Microsoft.EntityFrameworkCore.Tools
dotnet add package Microsoft.Data.SqlClient
```

3. Create the following folder structure:
```
WarehouseAPI/
├── Controllers/
├── Models/
├── Data/
└── DTOs/
```

4. Create model classes in the Models folder:
   - Product.cs
   - Warehouse.cs
   - Order.cs
   - ProductWarehouse.cs

5. Create the database context in the Data folder:
   - WarehouseContext.cs

6. Create the DTO in the DTOs folder:
   - ProductWarehouseRequest.cs

7. Create the controller in the Controllers folder:
   - WarehouseController.cs

8. Update Program.cs to configure services and middleware

9. Configure the database connection in appsettings.json:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=WarehouseDB;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## Database Setup

1. Create the database using Entity Framework migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

2. Insert sample data using the provided SQL scripts:
   - create.sql (creates tables)
   - proc.sql (creates stored procedures)

## Running the Application

1. Start the application:
```bash
dotnet run
```

2. The API will be available at:
   - https://localhost:7001 (or similar port)
   - Swagger UI: https://localhost:7001/swagger

## API Usage

### Add Product to Warehouse

**Endpoint:** POST /api/warehouse

**Request Body:**
```json
{
    "idProduct": 1,
    "idWarehouse": 1,
    "amount": 10,
    "createdAt": "2024-03-19T10:00:00"
}
```

**Response:**
- Success: Returns the ID of the created ProductWarehouse entry
- Error: Returns appropriate error message with status code

## Project Structure

- **Models/** - Contains entity classes
  - Product.cs - Product entity
  - Warehouse.cs - Warehouse entity
  - Order.cs - Order entity
  - ProductWarehouse.cs - Product-Warehouse relationship entity

- **Data/** - Contains database context
  - WarehouseContext.cs - Entity Framework DbContext

- **DTOs/** - Contains data transfer objects
  - ProductWarehouseRequest.cs - Request DTO for adding products to warehouse

- **Controllers/** - Contains API controllers
  - WarehouseController.cs - Handles warehouse-related operations


Example curl command:
```bash
curl -X POST "https://localhost:7001/api/warehouse" \
     -H "Content-Type: application/json" \
     -d '{"idProduct": 1, "idWarehouse": 1, "amount": 10, "createdAt": "2024-03-19T10:00:00"}'
``` 