using APBD_07.Models;

namespace APBD_07.Services;

public interface IClientService
{
    Task<IEnumerable<RegistrationDto>> GetClientTripsAsync(int clientId);
    Task<int> CreateClientAsync(ClientCreateDto dto);
    Task RegisterTripAsync(int clientId, int tripId);
    Task UnregisterTripAsync(int clientId, int tripId);
}