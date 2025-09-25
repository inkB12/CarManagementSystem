
using CarManagementSystem.DataAccess;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using CarManagementSystem.Services.Interfaces;

namespace CarManagementSystem.Services.Services
{
    public class CarCompanyService : ICarCompanyService
    {

        private readonly ICarCompanyRepository _repo;

        public CarCompanyService(ICarCompanyRepository repo)
        {
            _repo = repo;
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
            var ok = await _repo.DeleteAsync(id);
            return ok ? (true, "Deleted") : (false, "Not found");
        }
    }
}
