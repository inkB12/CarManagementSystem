using CarManagementSystem.BusinessObjects;
using CarManagementSystem.Services.Dtos.Momo;

namespace CarManagementSystem.Services.Interfaces
{
    public interface IMomoService
    {
        Task<MomoResponseDTO> CreatePaymentAsync(Order order);
    }
}
