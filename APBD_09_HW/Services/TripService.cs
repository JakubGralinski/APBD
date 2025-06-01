using System.Linq;
using System.Threading.Tasks;
using APBD_09_HW.DTOs;
using APBD_09_HW.Models;
using Microsoft.EntityFrameworkCore;

namespace APBD_09_HW.Services
{
    public class TripService : ITripService
    {
        private readonly AppDbContext _context;
        public TripService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedTripsResponseDto> GetTripsAsync(int page, int pageSize)
        {
            var query = _context.Trips
                .Include(t => t.CountryTrips).ThenInclude(ct => ct.Country)
                .Include(t => t.ClientTrips).ThenInclude(ct => ct.Client)
                .OrderByDescending(t => t.DateFrom);

            var totalTrips = await query.CountAsync();
            var allPages = (int)System.Math.Ceiling(totalTrips / (double)pageSize);
            var trips = await query.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            var tripDtos = trips.Select(t => new TripDto
            {
                Name = t.Name,
                Description = t.Description,
                DateFrom = t.DateFrom.ToString("yyyy-MM-dd"),
                DateTo = t.DateTo.ToString("yyyy-MM-dd"),
                MaxPeople = t.MaxPeople,
                Countries = t.CountryTrips.Select(ct => new CountryDto { Name = ct.Country.Name }).ToList(),
                Clients = t.ClientTrips.Select(ct => new ClientDto
                {
                    FirstName = ct.Client.FirstName,
                    LastName = ct.Client.LastName
                }).ToList()
            }).ToList();

            return new PaginatedTripsResponseDto
            {
                PageNum = page,
                PageSize = pageSize,
                AllPages = allPages,
                Trips = tripDtos
            };
        }

        public async Task<bool> AssignClientToTripAsync(int idTrip, AssignClientDto dto)
        {
            // 1. Check if client with given PESEL exists
            var client = await _context.Clients.FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);
            if (client == null)
            {
                // Create new client
                client = new Client
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    Email = dto.Email,
                    Telephone = dto.Telephone,
                    Pesel = dto.Pesel
                };
                _context.Clients.Add(client);
                await _context.SaveChangesAsync();
            }
            else
            {
                // If client exists, check if already registered for this trip
                bool alreadyRegistered = await _context.ClientTrips.AnyAsync(ct => ct.IdClient == client.IdClient && ct.IdTrip == idTrip);
                if (alreadyRegistered)
                    throw new System.Exception("Client with this PESEL is already registered for this trip.");
            }

            // 2. Check if trip exists and DateFrom is in the future
            var trip = await _context.Trips.FirstOrDefaultAsync(t => t.IdTrip == idTrip);
            if (trip == null)
                throw new System.Exception("Trip does not exist.");
            if (trip.DateFrom <= DateTime.Now)
                throw new System.Exception("Cannot register for a trip that has already started.");

            // 3. Register client for the trip
            var clientTrip = new ClientTrip
            {
                IdClient = client.IdClient,
                IdTrip = idTrip,
                RegisteredAt = DateTime.Now,
                PaymentDate = dto.PaymentDate
            };
            _context.ClientTrips.Add(clientTrip);
            await _context.SaveChangesAsync();
            return true;
        }
    }
} 