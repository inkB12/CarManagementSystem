
using System.Linq.Expressions;
using CarManagementSystem.BusinessObjects;


namespace CarManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface IElectricVehicleRepository
    {
        Task<ElectricVehicle> GetByIdAsync(int id);
        Task<List<ElectricVehicle>> GetAllAsync(Expression<Func<ElectricVehicle, bool>>? predicate = null);

        Task<ElectricVehicle> CreateAsync(ElectricVehicle vehicle);
        Task<ElectricVehicle> UpdateAsync(ElectricVehicle vehicle);
        Task<bool> DeleteAsync(int id);

        Task<int> SaveChangesAsync();
    }
}
