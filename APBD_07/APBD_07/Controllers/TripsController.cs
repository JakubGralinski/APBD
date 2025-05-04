using Microsoft.AspNetCore.Mvc;
using APBD_07.Services;

[ApiController]
[Route("api/[controller]")]
public class TripsController : ControllerBase
{
    private readonly ITripService _tripService;

    public TripsController(ITripService tripService)
        => _tripService = tripService;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var trips = await _tripService.GetAllAsync();
        return Ok(trips);
    }
}