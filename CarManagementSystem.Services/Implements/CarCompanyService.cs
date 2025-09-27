
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
            try
            {
                company.IsActive = true; // luôn mặc định true
                var saved = await _repo.CreateAsync(company);
                return (true, "Thêm hãng xe thành công", saved);
            }
            catch (Exception ex)
            {
                return (false, $"Thêm hãng xe thất bại: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string message, CarCompany? data)> UpdateAsync(CarCompany company)
        {
            var existed = await _repo.GetByIdAsync(company.Id);
            if (existed == null) return (false, "Không tìm thấy hãng xe", null);

            try
            {
                // ❌ Không cho cập nhật IsActive
                existed.CatalogName = company.CatalogName?.Trim();
                existed.Description = company.Description;

                var updated = await _repo.UpdateAsync(existed);
                return (true, "Cập nhật hãng xe thành công", updated);
            }
            catch (Exception ex)
            {
                return (false, $"Cập nhật hãng xe thất bại: {ex.Message}", null);
            }
        }

        public async Task<(bool ok, string message)> DeleteAsync(int id)
        {
            var company = await _repo.GetByIdAsync(id);
            if (company == null)
                return (false, "Không tìm thấy hãng xe");

            try
            {
                // ❌ Không xóa thật, chỉ set IsActive = false
                company.IsActive = false;
                await _repo.UpdateAsync(company);

                return (true, $"Đã ngưng hoạt động hãng xe {company.CatalogName}");
            }
            catch (Exception ex)
            {
                return (false, $"Xóa hãng xe thất bại: {ex.Message}");
            }
        }
    }
}
