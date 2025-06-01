using Microsoft.AspNetCore.Mvc;
using APBD_09_HW.Services;

namespace APBD_09_HW.Controllers
{
    [ApiController]
    [Route("api/clients")]
    public class ClientsController : ControllerBase
    {
        private readonly IClientService _clientService;
        public ClientsController(IClientService clientService)
        {
            _clientService = clientService;
        }

        // DELETE /api/clients/{idClient}
        [HttpDelete("{idClient}")]
        public async Task<IActionResult> DeleteClient(int idClient)
        {
            var result = await _clientService.DeleteClientAsync(idClient);
            if (!result)
                return BadRequest(new { error = "Client not found or has assigned trips." });
            return Ok(new { message = "Client deleted successfully." });
        }
    }
} 