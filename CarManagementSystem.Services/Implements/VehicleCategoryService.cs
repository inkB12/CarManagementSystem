using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Interfaces;


namespace CarManagementSystem.Services.Services
{
    public class VehicleCategoryService : IVehicleCategoryService
    {
        private readonly IVehicleCategoryRepository _repo;
        private readonly IElectricVehicleRepository _evRepo;

        public VehicleCategoryService(IVehicleCategoryRepository repo, IElectricVehicleRepository evRepo)
        {
            _repo = repo;
            _evRepo = evRepo;
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
            try
            {
                category.IsActive = true; // luôn true khi tạo mới
                var saved = await _repo.CreateAsync(category);
                return (true, "Thêm danh mục thành công", saved);
            }
            catch (Exception ex)
            {
                return (false, $"Thêm danh mục thất bại: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string message, VehicleCategory? data)> UpdateAsync(VehicleCategory category)
        {
            var existed = await _repo.GetByIdAsync(category.Id);
            if (existed == null) return (false, "Không tìm thấy danh mục", null);

            try
            {
                // ❌ Không update IsActive
                existed.CategoryName = category.CategoryName?.Trim();

                var updated = await _repo.UpdateAsync(existed);
                return (true, "Cập nhật danh mục thành công", updated);
            }
            catch (Exception ex)
            {
                return (false, $"Cập nhật danh mục thất bại: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            var category = await _repo.GetByIdAsync(id);
            if (category == null)
                return (false, "Không tìm thấy danh mục");

            // Kiểm tra ràng buộc với ElectricVehicle
            var vehicles = await _evRepo.GetAllAsync(ev => ev.VehicleCategoryId == id);
            if (vehicles.Any())
            {
                return (false, $"Không thể xóa danh mục {category.CategoryName} vì vẫn còn xe thuộc danh mục này");
            }

            try
            {
                // ❌ Xóa mềm
                category.IsActive = false;
                await _repo.UpdateAsync(category);

                return (true, $"Đã ngưng hoạt động danh mục {category.CategoryName}");
            }
            catch (Exception ex)
            {
                return (false, $"Xóa danh mục thất bại: {ex.Message}");
            }
        }
    }
}
