using System.Threading.Tasks;
using APBD_09_HW.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_09_HW.Services
{
    public class ClientService : IClientService
    {
        private readonly AppDbContext _context;
        public ClientService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> DeleteClientAsync(int idClient)
        {
            var client = await _context.Clients.Include(c => c.ClientTrips).FirstOrDefaultAsync(c => c.IdClient == idClient);
            if (client == null)
                return false;
            if (client.ClientTrips.Any())
                return false;
            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 