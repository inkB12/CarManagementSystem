
using System.Linq.Expressions;


namespace CarManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface ICarCompanyRepository
    {
        Task<CarCompany> GetByIdAsync(int id);
        Task<List<CarCompany>> GetAllAsync(Expression<Func<CarCompany, bool>>? predicate = null);

        Task<CarCompany> CreateAsync(CarCompany company);
        Task<CarCompany> UpdateAsync(CarCompany company);
        Task<bool> DeleteAsync(int id);

        Task<int> SaveChangesAsync();
    }
}
