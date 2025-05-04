namespace APBD_07.Services;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using APBD_07.Models;

public class ClientService : IClientService
{
    private readonly string _connectionString;

    public ClientService(IConfiguration configuration)
        => _connectionString = configuration.GetConnectionString("Default");

    public async Task<IEnumerable<RegistrationDto>> GetClientTripsAsync(int clientId)
    {
        const string checkSql = """
            SELECT COUNT(*) FROM Client WHERE Id = @ClientId;
        """;
        const string querySql = """
            SELECT ct.TripId, t.Name, ct.RegisteredAt, p.Amount
            FROM Client_Trip ct
            JOIN Trip t ON ct.TripId = t.Id
            LEFT JOIN Payment p ON p.ClientId = ct.ClientId AND p.TripId = ct.TripId
            WHERE ct.ClientId = @ClientId;
        """;

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        // Check existence
        await using (var chk = new SqlCommand(checkSql, conn))
        {
            chk.Parameters.AddWithValue("@ClientId", clientId);
            if ((int)await chk.ExecuteScalarAsync() == 0)
                throw new KeyNotFoundException("Client not found");
        }

        // Fetch registrations
        var regs = new List<RegistrationDto>();
        await using (var cmd = new SqlCommand(querySql, conn))
        {
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            await using var rd = await cmd.ExecuteReaderAsync();
            while (await rd.ReadAsync())
            {
                regs.Add(new RegistrationDto
                {
                    TripId        = rd.GetInt32(0),
                    TripName      = rd.GetString(1),
                    RegisteredAt  = rd.GetDateTime(2),
                    PaymentAmount = rd.IsDBNull(3) ? null : rd.GetDecimal(3)
                });
            }
        }

        return regs;
    }

    public async Task<int> CreateClientAsync(ClientCreateDto dto)
    {
        const string insertSql = """
            INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
            OUTPUT INSERTED.Id
            VALUES (@FirstName, @LastName, @Email, @Telephone, @Pesel);
        """;

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(insertSql, conn);
        cmd.Parameters.AddWithValue("@FirstName", dto.FirstName);
        cmd.Parameters.AddWithValue("@LastName",  dto.LastName);
        cmd.Parameters.AddWithValue("@Email",     dto.Email);
        cmd.Parameters.AddWithValue("@Telephone", dto.Telephone ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@Pesel",     dto.Pesel     ?? (object)DBNull.Value);

        return (int)await cmd.ExecuteScalarAsync();
    }

    public async Task RegisterTripAsync(int clientId, int tripId)
    {
        const string checkSql = """
            SELECT COUNT(*) FROM Client WHERE Id = @ClientId;
            SELECT COUNT(*) FROM Trip   WHERE Id = @TripId;
        """;
        const string capacitySql = """
            SELECT t.MaxParticipants, COUNT(ct.TripId)
            FROM Trip t
            LEFT JOIN Client_Trip ct ON ct.TripId = t.Id
            WHERE t.Id = @TripId
            GROUP BY t.MaxParticipants;
        """;
        const string insertSql = """
            INSERT INTO Client_Trip (ClientId, TripId, RegisteredAt)
            VALUES (@ClientId, @TripId, GETDATE());
        """;

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        // Existence checks
        await using (var chk = new SqlCommand(checkSql, conn))
        {
            chk.Parameters.AddWithValue("@ClientId", clientId);
            chk.Parameters.AddWithValue("@TripId",   tripId);
            await using var rd = await chk.ExecuteReaderAsync();
            rd.Read(); if (rd.GetInt32(0) == 0) throw new KeyNotFoundException("Client not found");
            rd.NextResult();
            rd.Read(); if (rd.GetInt32(0) == 0) throw new KeyNotFoundException("Trip not found");
        }

        // Capacity check
        await using (var cap = new SqlCommand(capacitySql, conn))
        {
            cap.Parameters.AddWithValue("@TripId", tripId);
            await using var rdCap = await cap.ExecuteReaderAsync();
            rdCap.Read();
            if (rdCap.GetInt32(1) >= rdCap.GetInt32(0))
                throw new InvalidOperationException("Trip is full");
        }

        // Register
        await using (var cmd = new SqlCommand(insertSql, conn))
        {
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@TripId",   tripId);
            await cmd.ExecuteNonQueryAsync();
        }
    }

    public async Task UnregisterTripAsync(int clientId, int tripId)
    {
        const string deleteSql = """
            DELETE FROM Client_Trip
            WHERE ClientId = @ClientId AND TripId = @TripId;
        """;

        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        // Ensure registration exists
        await using (var chk = new SqlCommand(
            "SELECT COUNT(*) FROM Client_Trip WHERE ClientId=@ClientId AND TripId=@TripId;", conn))
        {
            chk.Parameters.AddWithValue("@ClientId", clientId);
            chk.Parameters.AddWithValue("@TripId",   tripId);
            if ((int)await chk.ExecuteScalarAsync() == 0)
                throw new KeyNotFoundException("Registration not found");
        }

        // Delete
        await using (var cmd = new SqlCommand(deleteSql, conn))
        {
            cmd.Parameters.AddWithValue("@ClientId", clientId);
            cmd.Parameters.AddWithValue("@TripId",   tripId);
            await cmd.ExecuteNonQueryAsync();
        }
    }
}