namespace APBD_07.Services;

using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using APBD_07.Models;

public class TripService : ITripService
{
    private readonly string _connectionString;
    public TripService(IConfiguration config)
        => _connectionString = config.GetConnectionString("Default");

    public async Task<IEnumerable<TripDto>> GetAllAsync()
    {
        const string sql = """
                               SELECT t.Id, t.Name, t.Description, t.StartDate, t.EndDate, t.MaxParticipants, c.Name
                               FROM Trip t
                               JOIN Country c ON t.CountryId = c.Id;
                           """;

        var list = new List<TripDto>();
        await using var conn = new SqlConnection(_connectionString);
        await conn.OpenAsync();

        await using var cmd = new SqlCommand(sql, conn);
        await using var rd  = await cmd.ExecuteReaderAsync();
        while (await rd.ReadAsync())
        {
            list.Add(new TripDto
            {
                Id              = rd.GetInt32(0),
                Name            = rd.GetString(1),
                Description     = rd.GetString(2),
                StartDate       = rd.GetDateTime(3),
                EndDate         = rd.GetDateTime(4),
                MaxParticipants = rd.GetInt32(5),
                Country         = rd.GetString(6)
            });
        }

        return list;
    }
}