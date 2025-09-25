using CarManagementSystem.DataAccess;

namespace CarManagementSystem.Services.Interfaces
{
    public interface ICarCompanyService
    {
        Task<List<CarCompany>> GetAllAsync(bool onlyActive = true);
        Task<CarCompany?> GetByIdAsync(int id);

        Task<(bool ok, string message, CarCompany? data)> CreateAsync(CarCompany company);
        Task<(bool ok, string message, CarCompany? data)> UpdateAsync(CarCompany company);
        Task<(bool ok, string message)> DeleteAsync(int id);
    }
}
