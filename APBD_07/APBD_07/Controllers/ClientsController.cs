using APBD_07.Models;
using Microsoft.AspNetCore.Mvc;
using APBD_07.Models;
using APBD_07.Services;

[ApiController]
[Route("api/[controller]")]
public class ClientsController : ControllerBase
{
    private readonly IClientService _clientService;

    public ClientsController(IClientService clientService)
        => _clientService = clientService;

    [HttpGet("{id}/trips")]
    public async Task<IActionResult> GetClientTrips(int id)
    {
        try
        {
            var trips = await _clientService.GetClientTripsAsync(id);
            return Ok(trips);
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateClient([FromBody] ClientCreateDto dto)
    {
        int newId = await _clientService.CreateClientAsync(dto);
        return CreatedAtAction(nameof(GetClientTrips), new { id = newId }, new { Id = newId });
    }

    [HttpPut("{id}/trips/{tripId}")]
    public async Task<IActionResult> RegisterTrip(int id, int tripId)
    {
        try
        {
            await _clientService.RegisterTripAsync(id, tripId);
            return NoContent();
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
        catch (InvalidOperationException ioe)
        {
            return Conflict(ioe.Message);
        }
    }

    [HttpDelete("{id}/trips/{tripId}")]
    public async Task<IActionResult> UnregisterTrip(int id, int tripId)
    {
        try
        {
            await _clientService.UnregisterTripAsync(id, tripId);
            return NoContent();
        }
        catch (KeyNotFoundException knf)
        {
            return NotFound(knf.Message);
        }
    }
}