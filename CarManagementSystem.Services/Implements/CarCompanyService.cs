
using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Interfaces;

namespace CarManagementSystem.Services.Services
{
    public class CarCompanyService : ICarCompanyService
    {

        private readonly ICarCompanyRepository _repo;
        private readonly IElectricVehicleRepository _evRepo;

        public CarCompanyService(ICarCompanyRepository repo, IElectricVehicleRepository evRepo)
        {
            _repo = repo;
            _evRepo = evRepo;
        }

        public async Task<List<CarCompany>> GetAllAsync(bool onlyActive = true)
        {
            if (onlyActive)
            {
                return await _repo.GetAllAsync(c => c.IsActive == true);
            }
            return await _repo.GetAllAsync();
        }

        public Task<CarCompany?> GetByIdAsync(int id)
        {
            return _repo.GetByIdAsync(id);
        }

        public async Task<(bool ok, string message, CarCompany? data)> CreateAsync(CarCompany company)
        {
            company.IsActive = true;
            var saved = await _repo.CreateAsync(company);
            return (true, "Created", saved);
        }

        public async Task<(bool ok, string message, CarCompany? data)> UpdateAsync(CarCompany company)
        {
            var existed = await _repo.GetByIdAsync(company.Id);
            if (existed == null) return (false, "Not found", null);

            existed.CatalogName = company.CatalogName?.Trim();
            existed.Description = company.Description;
            existed.IsActive = company.IsActive;

            var updated = await _repo.UpdateAsync(existed);
            return (true, "Updated", updated);
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            var company = await _repo.GetByIdAsync(id);
            if (company == null)
                return (false, "Không tìm thấy hãng xe");

            // ✅ Kiểm tra có ElectricVehicle nào tham chiếu tới CarCompanyId này không
            var vehicles = await _evRepo.GetAllAsync(ev => ev.CarCompanyId == id);
            if (vehicles.Any())
            {
                return (false, $"Không thể xóa hãng xe {company.CatalogName} vì vẫn còn xe thuộc hãng này");
            }

            var ok = await _repo.DeleteAsync(id);
            return ok ? (true, "Đã xóa thành công") : (false, "Xóa thất bại");
        }
    }
}
