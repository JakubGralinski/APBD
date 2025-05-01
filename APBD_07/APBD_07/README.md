# Travel Agency API

A REST API for a travel agency database implementing CRUD operations using ADO.NET and SQL Server.


## Configuration

1. Create a `.env` file in the root directory with your database credentials:
```
DB_PASSWORD=your_password_here
```

2. The connection string is configured in `appsettings.json`:
```json
"ConnectionStrings": {
  "Default": "Data Source=db-mssql;Initial Catalog=2019SBD;Integrated Security=True;Trust Server Certificate=True"
}
```

## Running the Application

1. Restore NuGet packages:
```bash
dotnet restore
```

2. Run the application:
```bash
dotnet run
```

## Endpoints

1. `GET /api/trips` - Get all available trips
2. `GET /api/clients/{id}/trips` - Get trips for a specific client
3. `POST /api/clients` - Create a new client
4. `PUT /api/clients/{id}/trips/{tripId}` - Register a client for a trip
5. `DELETE /api/clients/{id}/trips/{tripId}` - Unregister a client from a trip

