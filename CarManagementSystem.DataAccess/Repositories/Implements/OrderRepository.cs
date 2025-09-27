

using System.Linq.Expressions;
using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

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
            await SaveChangeAsync();
            return order;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var order = await GetByIdAsync(id);

            if (order == null)
            {
                return false;
            }

            order.Status = "CANCELLED";

            await SaveChangeAsync();

            return true;
        }

        public async Task<List<Order>> GetAllAsync(Expression<Func<Order, bool>>? predicate = null)
        {
            IQueryable<Order> query = _context.Orders.AsNoTracking();
            if (predicate != null) query = query.Where(predicate);
            return await query.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Order> GetByIdAsync(int id) => await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);

        public Task<int> SaveChangeAsync() => _context.SaveChangesAsync();


        public async Task<Order> UpdateAsync(Order order)
        {
            _context.Orders.Update(order);
            await SaveChangeAsync();
            return order;
        }
    }
}
