using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Interfaces;


namespace CarManagementSystem.Services.Services
{
    public class ElectricVehicleService : IElectricVehicleService
    {
        private readonly IElectricVehicleRepository _repo;
        private readonly ICarCompanyRepository _carCompanyRepo;
        private readonly IVehicleCategoryRepository _categoryRepo;

        public ElectricVehicleService(
            IElectricVehicleRepository repo,
            ICarCompanyRepository carCompanyRepo,
            IVehicleCategoryRepository categoryRepo)
        {
            _repo = repo;
            _carCompanyRepo = carCompanyRepo;
            _categoryRepo = categoryRepo;
        }

        public async Task<List<ElectricVehicle>> GetAllAsync(bool onlyActive = true)
        {
            if (onlyActive)
            {
                return await _repo.GetAllAsync(ev => ev.IsActive == true);
            }
            return await _repo.GetAllAsync();
        }

        public Task<ElectricVehicle?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public async Task<(bool ok, string message, ElectricVehicle? data)> CreateAsync(ElectricVehicle vehicle)
        {
            try
            {
                // kiểm tra FK
                var company = await _carCompanyRepo.GetByIdAsync(vehicle.CarCompanyId);
                if (company == null || !company.IsActive) return (false, "Hãng xe không tồn tại hoặc đã ngừng hoạt động", null);

                var category = await _categoryRepo.GetByIdAsync(vehicle.VehicleCategoryId);
                if (category == null || !category.IsActive) return (false, "Danh mục không tồn tại hoặc đã ngừng hoạt động", null);

                vehicle.IsActive = true;
                var saved = await _repo.CreateAsync(vehicle);
                return (true, "Thêm xe thành công", saved);
            }
            catch (Exception ex)
            {
                return (false, $"Thêm xe thất bại: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string message, ElectricVehicle? data)> UpdateAsync(ElectricVehicle vehicle)
        {
            var existed = await _repo.GetByIdAsync(vehicle.Id);
            if (existed == null) return (false, "Không tìm thấy xe", null);

            try
            {
                // kiểm tra FK
                var company = await _carCompanyRepo.GetByIdAsync(vehicle.CarCompanyId);
                if (company == null || !company.IsActive) return (false, "Hãng xe không hợp lệ", null);

                var category = await _categoryRepo.GetByIdAsync(vehicle.VehicleCategoryId);
                if (category == null || !category.IsActive) return (false, "Danh mục không hợp lệ", null);

                // cập nhật field (KHÔNG đụng IsActive)
                existed.Model = vehicle.Model?.Trim();
                existed.Version = vehicle.Version;
                existed.Price = vehicle.Price;
                existed.Specification = vehicle.Specification;
                existed.Color = vehicle.Color;
                existed.ImageUrl = vehicle.ImageUrl;
                existed.CarCompanyId = vehicle.CarCompanyId;
                existed.VehicleCategoryId = vehicle.VehicleCategoryId;

                var updated = await _repo.UpdateAsync(existed);
                return (true, "Cập nhật xe thành công", updated);
            }
            catch (Exception ex)
            {
                return (false, $"Cập nhật xe thất bại: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            var vehicle = await _repo.GetByIdAsync(id);
            if (vehicle == null) return (false, "Không tìm thấy xe");

            try
            {
                vehicle.IsActive = false; // ❌ Xóa mềm
                await _repo.UpdateAsync(vehicle);
                return (true, $"Đã ngưng hoạt động xe {vehicle.Model}");
            }
            catch (Exception ex)
            {
                return (false, $"Xóa xe thất bại: {ex.Message}");
            }
        }
    }
}
