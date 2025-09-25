

using System.Linq.Expressions;

namespace CarManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface IPromotionRepository
    {
        Task<Promotion> GetByIdAsync(int id);
        Task<List<Promotion>> GetAllAsync(Expression<Func<Promotion, bool>>? predicate = null);

        Task<Promotion> CreateAsync(Promotion promotion);
        Task<Promotion> UpdateAsync(Promotion promotion);
        Task<bool> DeleteAsync(int id);

        Task<int> SaveChangesAsync();
    }
}
