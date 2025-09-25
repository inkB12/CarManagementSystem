using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Interfaces;


namespace CarManagementSystem.Services.Services
{
    public class VehicleCategoryService : IVehicleCategoryService
    {
        private readonly IVehicleCategoryRepository _repo;

        public VehicleCategoryService(IVehicleCategoryRepository repo)
        {
            _repo = repo;
        }

        public async Task<List<VehicleCategory>> GetAllAsync(bool onlyActive = true)
        {
            if (onlyActive)
            {
                return await _repo.GetAllAsync(c => c.IsActive == true);
            }
            return await _repo.GetAllAsync();
        }

        public Task<VehicleCategory?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public async Task<(bool ok, string message, VehicleCategory? data)> CreateAsync(VehicleCategory category)
        {
            category.IsActive = true;
            var saved = await _repo.CreateAsync(category);
            return (true, "Created", saved);
        }

        public async Task<(bool ok, string message, VehicleCategory? data)> UpdateAsync(VehicleCategory category)
        {
            var existed = await _repo.GetByIdAsync(category.Id);
            if (existed == null) return (false, "Not found", null);

            existed.CategoryName = category.CategoryName?.Trim();
            existed.IsActive = category.IsActive;

            var updated = await _repo.UpdateAsync(existed);
            return (true, "Updated", updated);
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            var ok = await _repo.DeleteAsync(id);
            return ok ? (true, "Deleted") : (false, "Not found");
        }
    }
}
