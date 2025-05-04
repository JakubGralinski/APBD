using APBD_07.Models;

namespace APBD_07.Services;

public interface ITripService
{
    Task<IEnumerable<TripDto>> GetAllAsync();
}
