using System.Threading.Tasks;

namespace APBD_09_HW.Services
{
    public interface IClientService
    {
        Task<bool> DeleteClientAsync(int idClient);
    }
} 