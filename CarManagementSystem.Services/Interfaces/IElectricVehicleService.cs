

using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess;

namespace CarManagementSystem.Services.Interfaces
{
    public interface IElectricVehicleService
    {
        Task<List<ElectricVehicle>> GetAllAsync(bool onlyActive = true);
        Task<ElectricVehicle?> GetByIdAsync(int id);

        Task<(bool ok, string message, ElectricVehicle? data)> CreateAsync(ElectricVehicle vehicle);
        Task<(bool ok, string message, ElectricVehicle? data)> UpdateAsync(ElectricVehicle vehicle);
        Task<(bool ok, string message)> DeleteAsync(int id);
    }
}
