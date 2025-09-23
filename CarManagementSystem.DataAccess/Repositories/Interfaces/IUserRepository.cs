using System.Linq.Expressions;

namespace CarManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetByIdAsync(int id);
        Task<User> GetByEmailAsync(string email);
        Task<List<User>> GetAllAsync(Expression<Func<User, bool>>? predicate = null);

        Task<User> CreateAsync(User user);
        Task<User> UpdateAsync(User user);
        Task<bool> DeleteAsync(int id);

        Task<int> SaveChangesAsync();
    }
}
