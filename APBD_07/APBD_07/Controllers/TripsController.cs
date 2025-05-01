using APBD_07.Models;

namespace APBD_07.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly string _conn;

    public TripsController(IConfiguration cfg)
        => _conn = cfg.GetConnectionString("Default");

    // GET /api/trips
    [HttpGet]
    public IActionResult GetAll()
    {
        var trips = new List<TripDto>();
        using var cn = new SqlConnection(_conn);
        using var cmd = new SqlCommand(@"
            SELECT t.Id, t.Name, t.Description, t.StartDate, t.EndDate, t.MaxParticipants, c.Name AS Country
            FROM Trip t
            JOIN Country c ON t.CountryId = c.Id", cn);
        cn.Open();
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            trips.Add(new TripDto {
                Id = rd.GetInt32(0),
                Name = rd.GetString(1),
                Description = rd.GetString(2),
                StartDate = rd.GetDateTime(3),
                EndDate = rd.GetDateTime(4),
                MaxParticipants = rd.GetInt32(5),
                Country = rd.GetString(6)
            });
        }
        return Ok(trips);
    }
}