

using System.Linq.Expressions;

namespace CarManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface IFeedbackRepository
    {
        Task<Feedback> GetByIdAsync(int id);
        Task<List<Feedback>> GetAllAsync(Expression<Func<Feedback, bool>>? predicate = null);

        Task<Feedback> CreateAsync(Feedback feedback);
        Task<Feedback> UpdateAsync(Feedback feedback);
        Task<bool> DeleteAsync(int id);

        Task<int> SaveChangesAsync();
    }
}
