using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess;
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
            // Kiểm tra CarCompany tồn tại
            var company = await _carCompanyRepo.GetByIdAsync(vehicle.CarCompanyId);
            if (company == null) return (false, "CarCompany not found", null);

            // Kiểm tra Category tồn tại
            var category = await _categoryRepo.GetByIdAsync(vehicle.VehicleCategoryId);
            if (category == null) return (false, "VehicleCategory not found", null);

            vehicle.IsActive = true;
            var saved = await _repo.CreateAsync(vehicle);
            return (true, "Created", saved);
        }

        public async Task<(bool ok, string message, ElectricVehicle? data)> UpdateAsync(ElectricVehicle vehicle)
        {
            var existed = await _repo.GetByIdAsync(vehicle.Id);
            if (existed == null) return (false, "Not found", null);

            // validate FK
            var company = await _carCompanyRepo.GetByIdAsync(vehicle.CarCompanyId);
            if (company == null) return (false, "CarCompany not found", null);

            var category = await _categoryRepo.GetByIdAsync(vehicle.VehicleCategoryId);
            if (category == null) return (false, "VehicleCategory not found", null);

            // cập nhật field
            existed.Model = vehicle.Model?.Trim();
            existed.Version = vehicle.Version;
            existed.Specification = vehicle.Specification;
            existed.Price = vehicle.Price;
            existed.Color = vehicle.Color;
            existed.ImageUrl = vehicle.ImageUrl;
            existed.IsActive = vehicle.IsActive;
            existed.CarCompanyId = vehicle.CarCompanyId;
            existed.VehicleCategoryId = vehicle.VehicleCategoryId;

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
