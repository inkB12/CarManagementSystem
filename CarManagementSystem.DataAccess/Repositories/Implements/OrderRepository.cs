

using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarManagementSystem.DataAccess.Repositories.Implements
{
    public class OrderRepository : IOrderRepository
    {
        private readonly CarManagementDbContext _context;

        public OrderRepository(CarManagementDbContext context)
        {
            _context = context;
        }

        public async Task<Order> CreateAsync(Order order)
        {
            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            return order;
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

        public async Task<List<Order>> GetOrdersInRangeByStatusAsync(DateTime start, DateTime end, string status)
        {
            return await _context.Orders
        .AsNoTracking()
        .Include(o => o.User)
        .Where(o => o.Datetime >= start
                 && o.Datetime < end
                 && o.Status == status
                 && o.User.Role == "Customer") // ✅ chỉ lấy đơn của Customer
        .ToListAsync();
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
