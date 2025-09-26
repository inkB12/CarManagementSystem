using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess;

namespace CarManagementSystem.Services.Interfaces
{
    public interface IVehicleCategoryService
    {
        Task<List<VehicleCategory>> GetAllAsync(bool onlyActive = true);
        Task<VehicleCategory?> GetByIdAsync(int id);

        Task<(bool ok, string message, VehicleCategory? data)> CreateAsync(VehicleCategory category);
        Task<(bool ok, string message, VehicleCategory? data)> UpdateAsync(VehicleCategory category);
        Task<(bool ok, string message)> DeleteAsync(int id);
    }
}
