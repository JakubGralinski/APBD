using System.Threading.Tasks;
using APBD_09_HW.DTOs;

namespace APBD_09_HW.Services
{
    public interface ITripService
    {
        Task<PaginatedTripsResponseDto> GetTripsAsync(int page, int pageSize);
        Task<bool> AssignClientToTripAsync(int idTrip, AssignClientDto dto);
    }
} 