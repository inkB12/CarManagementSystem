
using System.Linq.Expressions;
using CarManagementSystem.BusinessObjects;

namespace CarManagementSystem.DataAccess.Repositories.Interfaces
{
    public interface IOrderRepository
    {
        Task<Order> GetByIdAsync(int id);
        Task<List<Order>> GetAllAsync(Expression<Func<Order, bool>>? predicate = null);
        Task<Order> CreateAsync(Order order);
        Task<Order> UpdateAsync(Order order);
        Task<bool> DeleteAsync(int id);
        Task<int> SaveChangeAsync();
    }
}
