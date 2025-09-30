

using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess;

namespace CarManagementSystem.Services.Interfaces
{
    public interface IPromotionService
    {
        Task<List<Promotion>> GetAllAsync(bool onlyActive = true);
        Task<Promotion?> GetByIdAsync(int id);

        Task<(bool ok, string message, Promotion? data)> CreateAsync(Promotion promotion);
        Task<(bool ok, string message, Promotion? data)> UpdateAsync(Promotion promotion);
        Task<(bool ok, string message)> DeleteAsync(int id);
    }
}
