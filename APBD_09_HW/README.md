# APBD_09_HW


### 1. **Project Initialization**
- Create a new ASP.NET Core Web API project.
   ```sh
    dotnet add package Microsoft.EntityFrameworkCore.SqlServer
    dotnet add package Microsoft.EntityFrameworkCore.Design
    dotnet add package Swashbuckle.AspNetCore
   ```

### 2. **Database and Models**

   ```sh
   docker compose up -d
   ```
start a SQL Server instance on `localhost:1433` with user `sa` and password `dummy_pass@123`.

---

## Db and Schema

1. Connect to the SQL Server
    - **Host:** `localhost`
    - **Port:** `1433`
    - **User:** `sa`
    - **Password:** `dummy_pass@123`

2. Create the database:
   ```sql
   CREATE DATABASE Tutorial9;
   ```

3. Switch to the new database and run the schema script:
    - Open `cw7_create (1).sql` and execute its contents in the `Tutorial9` database.

---

## the Connection String

The connection string is already set in `appsettings.Development.json`:
```
"DefaultConnection": "Server=localhost,1433;Database=Tutorial9;User ID=sa;Password=dummy_pass@123;Trust Server Certificate=True;"
```

---

## Scaffold EF Core Models

```sh
dotnet ef dbcontext scaffold "Server=localhost,1433;Database=Tutorial9;User ID=sa;Password=dummy_pass@123;Trust Server Certificate=True;" Microsoft.EntityFrameworkCore.SqlServer --context AppDbContext --output-dir Models --force --no-onconfiguring
```

---

### 3. **Directory Structure**
- Following folders:
    - `Models/` (for entity classes and `AppDbContext`)
    - `DTOs/` (for Data Transfer Objects)
    - `Services/` (for business logic/services)
    - `Controllers/` (for API controllers)

### 4. **DTOs**
- Create DTOs for API input/output:
    - `TripDto`, `CountryDto`, `ClientDto` (for trip listing)
    - `AssignClientDto` (for assigning a client to a trip)
    - `PaginatedTripsResponseDto` (for paginated trip responses)

### 5. **Services**
- Service interfaces:
    - `ITripService` (for trip-related logic)
    - `IClientService` (for client-related logic)
- Services:
    - `TripService` (trip listing, client assignment)
    - `ClientService` (client deletion with validation)

### 6. **Dependency Injection**
- Register services and `AppDbContext` in `Program.cs`:
  ```csharp
  builder.Services.AddDbContext<AppDbContext>(options =>
      options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
  builder.Services.AddScoped<ITripService, TripService>();
  builder.Services.AddScoped<IClientService, ClientService>();
  builder.Services.AddControllers();
  ```

### 7. **Controllers**
- API controllers:
    - `TripsController` (for `/api/trips` endpoints)
    - `ClientsController` (for `/api/clients` endpoints)
- endpoints:
    - `GET /api/trips` (with pagination and sorting)
    - `POST /api/trips/{idTrip}/clients` (assign client to trip)
    - `DELETE /api/clients/{idClient}` (delete client with checks)

### 8. **Business Logic**
- In `TripService`:
    - Trip listing with pagination and sorting.
    - Client assignment with all validation rules.
- In `ClientService`:
    - Client deletion with check for assigned trips.
