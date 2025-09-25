

using System.Linq.Expressions;
using CarManagementSystem.DataAccess.Repositories.Interfaces;

namespace CarManagementSystem.DataAccess.Repositories.Implements
{
    public class OrderRepository : IOrderRepository
    {
        public Task<Order> CreateAsync(Order order)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Order>> GetAllAsync(Expression<Func<Order, bool>>? predicate = null)
        {
            throw new NotImplementedException();
        }

        public Task<Order> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<int> SaveChangeAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Order> UpdateAsync(Order order)
        {
            throw new NotImplementedException();
        }
    }
}
