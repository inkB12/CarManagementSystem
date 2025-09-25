
using System.Linq.Expressions;


namespace CarManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface IVehicleCategoryRepository
    {
        Task<VehicleCategory> GetByIdAsync(int id);
        Task<List<VehicleCategory>> GetAllAsync(Expression<Func<VehicleCategory, bool>>? predicate = null);

        Task<VehicleCategory> CreateAsync(VehicleCategory category);
        Task<VehicleCategory> UpdateAsync(VehicleCategory category);
        Task<bool> DeleteAsync(int id);

        Task<int> SaveChangesAsync();
    }
}
