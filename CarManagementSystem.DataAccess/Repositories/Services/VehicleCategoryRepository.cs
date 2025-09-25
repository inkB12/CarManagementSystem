

using CarManagementSystem.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarManagementSystem.DataAccess.Repositories.Services
{
    public class VehicleCategoryRepository : IVehicleCategoryRepository
    {
        private readonly CarManagementDbContext _db;

        public VehicleCategoryRepository(CarManagementDbContext db)
        {
            _db = db;
        }

        public async Task<VehicleCategory> GetByIdAsync(int id) =>
            await _db.VehicleCategories.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<VehicleCategory>> GetAllAsync(Expression<Func<VehicleCategory, bool>>? predicate = null)
        {
            IQueryable<VehicleCategory> q = _db.VehicleCategories.AsNoTracking();
            if (predicate != null) q = q.Where(predicate);
            return await q.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<VehicleCategory> CreateAsync(VehicleCategory category)
        {
            _db.VehicleCategories.Add(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<VehicleCategory> UpdateAsync(VehicleCategory category)
        {
            _db.VehicleCategories.Update(category);
            await _db.SaveChangesAsync();
            return category;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var c = await _db.VehicleCategories.FindAsync(id);
            if (c == null) return false;

            _db.VehicleCategories.Remove(c);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
