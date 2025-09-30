

using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CarManagementSystem.DataAccess.Repositories.Services
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly CarManagementDbContext _db;

        public PromotionRepository(CarManagementDbContext db)
        {
            _db = db;
        }

        public async Task<Promotion> GetByIdAsync(int id) =>
            await _db.Promotions.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<Promotion>> GetAllAsync(Expression<Func<Promotion, bool>>? predicate = null)
        {
            IQueryable<Promotion> q = _db.Promotions.AsNoTracking();
            if (predicate != null) q = q.Where(predicate);
            return await q.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<Promotion> CreateAsync(Promotion promotion)
        {
            _db.Promotions.Add(promotion);
            await _db.SaveChangesAsync();
            return promotion;
        }

        public async Task<Promotion> UpdateAsync(Promotion promotion)
        {
            _db.Promotions.Update(promotion);
            await _db.SaveChangesAsync();
            return promotion;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var p = await _db.Promotions.FindAsync(id);
            if (p == null) return false;

            p.Status = "InActive";
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
