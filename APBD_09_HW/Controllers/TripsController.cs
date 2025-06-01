using Microsoft.AspNetCore.Mvc;
using APBD_09_HW.Services;
using APBD_09_HW.DTOs;

namespace APBD_09_HW.Controllers
{
    [ApiController]
    [Route("api/trips")]
    public class TripsController : ControllerBase
    {
        private readonly ITripService _tripService;
        public TripsController(ITripService tripService)
        {
            _tripService = tripService;
        }

        // GET /api/trips
        [HttpGet]
        public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _tripService.GetTripsAsync(page, pageSize);
            return Ok(result);
        }

        // POST /api/trips/{idTrip}/clients
        [HttpPost("{idTrip}/clients")]
        public async Task<IActionResult> AssignClientToTrip(int idTrip, [FromBody] AssignClientDto dto)
        {
            try
            {
                await _tripService.AssignClientToTripAsync(idTrip, dto);
                return Ok(new { message = "Client assigned to trip successfully." });
            }
            catch (System.Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }
} 