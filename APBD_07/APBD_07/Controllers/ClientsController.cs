using APBD_07.Models;

namespace APBD_07.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly string _conn;
    public ClientsController(IConfiguration cfg)
        => _conn = cfg.GetConnectionString("Default");

    // 2) GET /api/clients/{id}/trips
    [HttpGet("{id}/trips")]
    public IActionResult GetClientTrips(int id)
    {
        // Check client exists
        using var cn = new SqlConnection(_conn);
        using var chk = new SqlCommand("SELECT COUNT(*) FROM Client WHERE Id=@id", cn);
        chk.Parameters.AddWithValue("@id", id);
        cn.Open();
        if ((int)chk.ExecuteScalar() == 0)
            return NotFound("Client not found");

        // Retrieve trips + registration info
        var regs = new List<RegistrationDto>();
        using var cmd = new SqlCommand(@"
            SELECT ct.TripId, t.Name, ct.RegisteredAt, p.Amount
            FROM Client_Trip ct
            JOIN Trip t ON ct.TripId = t.Id
            LEFT JOIN Payment p ON p.ClientId = ct.ClientId AND p.TripId = ct.TripId
            WHERE ct.ClientId = @id", cn);
        cmd.Parameters.AddWithValue("@id", id);
        using var rd = cmd.ExecuteReader();
        while (rd.Read())
        {
            regs.Add(new RegistrationDto {
                TripId = rd.GetInt32(0),
                TripName = rd.GetString(1),
                RegisteredAt = rd.GetDateTime(2),
                PaymentAmount = rd.IsDBNull(3) ? null : rd.GetDecimal(3)
            });
        }
        return Ok(regs);
    }

    // 3) POST /api/clients
    [HttpPost]
    public IActionResult CreateClient([FromBody] ClientCreateDto dto)
    {
        // Basic validation
        if (string.IsNullOrWhiteSpace(dto.FirstName) ||
            string.IsNullOrWhiteSpace(dto.LastName)  ||
            string.IsNullOrWhiteSpace(dto.Email))
            return BadRequest("FirstName, LastName and Email are required");

        using var cn = new SqlConnection(_conn);
        using var cmd = new SqlCommand(@"
            INSERT INTO Client (FirstName, LastName, Email, Telephone, Pesel)
            OUTPUT INSERTED.Id
            VALUES (@fn,@ln,@em,@tel,@ps)", cn);
        cmd.Parameters.AddWithValue("@fn", dto.FirstName);
        cmd.Parameters.AddWithValue("@ln", dto.LastName);
        cmd.Parameters.AddWithValue("@em", dto.Email);
        cmd.Parameters.AddWithValue("@tel", dto.Telephone ?? (object)DBNull.Value);
        cmd.Parameters.AddWithValue("@ps", dto.Pesel ?? (object)DBNull.Value);

        cn.Open();
        var newId = (int)cmd.ExecuteScalar();
        return CreatedAtAction(nameof(GetClientTrips), new { id = newId }, new { Id = newId });
    }

    // 4) PUT /api/clients/{id}/trips/{tripId}
    [HttpPut("{id}/trips/{tripId}")]
    public async Task<IActionResult> RegisterTrip(int id, int tripId)
    {
        using var cn = new SqlConnection(_conn);
        await cn.OpenAsync();

        // 4.1 Check client & trip exist
        using var chk = new SqlCommand(
            "SELECT COUNT(*) FROM Client WHERE Id=@id; " +
            "SELECT COUNT(*) FROM Trip WHERE Id=@t;", cn);
        chk.Parameters.AddWithValue("@id", id);
        chk.Parameters.AddWithValue("@t", tripId);
        using var rdChk = await chk.ExecuteReaderAsync();
        rdChk.Read();
        if (rdChk.GetInt32(0) == 0) return NotFound("Client not found");
        rdChk.NextResult();
        rdChk.Read();
        if (rdChk.GetInt32(0) == 0) return NotFound("Trip not found");
        rdChk.Close();

        // 4.2 Check capacity
        using var cap = new SqlCommand(@"
            SELECT t.MaxParticipants, COUNT(ct.TripId) 
            FROM Trip t
            LEFT JOIN Client_Trip ct ON ct.TripId = t.Id
            WHERE t.Id = @t
            GROUP BY t.MaxParticipants", cn);
        cap.Parameters.AddWithValue("@t", tripId);
        using var rdCap = await cap.ExecuteReaderAsync();
        rdCap.Read();
        if (rdCap.GetInt32(1) >= rdCap.GetInt32(0))
            return Conflict("Trip is full");
        rdCap.Close();

        // 4.3 Insert registration
        using var cmd = new SqlCommand(@"
            INSERT INTO Client_Trip (ClientId, TripId, RegisteredAt)
            VALUES (@id,@t, GETDATE())", cn);
        cmd.Parameters.AddWithValue("@id", id);
        cmd.Parameters.AddWithValue("@t", tripId);
        await cmd.ExecuteNonQueryAsync();

        return NoContent();
    }

    // 5) DELETE /api/clients/{id}/trips/{tripId}
    [HttpDelete("{id}/trips/{tripId}")]
    public IActionResult UnregisterTrip(int id, int tripId)
    {
        using var cn = new SqlConnection(_conn);
        using var cmdCheck = new SqlCommand(@"
            SELECT COUNT(*) FROM Client_Trip 
            WHERE ClientId=@id AND TripId=@t", cn);
        cmdCheck.Parameters.AddWithValue("@id", id);
        cmdCheck.Parameters.AddWithValue("@t", tripId);
        cn.Open();
        if ((int)cmdCheck.ExecuteScalar() == 0)
            return NotFound("Registration not found");

        using var cmdDel = new SqlCommand(@"
            DELETE FROM Client_Trip
            WHERE ClientId=@id AND TripId=@t", cn);
        cmdDel.Parameters.AddWithValue("@id", id);
        cmdDel.Parameters.AddWithValue("@t", tripId);
        cmdDel.ExecuteNonQuery();
        return NoContent();
    }
}