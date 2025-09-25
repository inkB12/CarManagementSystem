
using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace CarManagementSystem.DataAccess.Repositories.Services
{
    public class ElectricVehicleRepository : IElectricVehicleRepository
    {
        private readonly CarManagementDbContext _db;

        public ElectricVehicleRepository(CarManagementDbContext db)
        {
            _db = db;
        }

        public async Task<ElectricVehicle> GetByIdAsync(int id) =>
            await _db.ElectricVehicles
                     .Include(ev => ev.CarCompany)
                     .Include(ev => ev.VehicleCategory)
                     .FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<ElectricVehicle>> GetAllAsync(Expression<Func<ElectricVehicle, bool>>? predicate = null)
        {
            IQueryable<ElectricVehicle> q = _db.ElectricVehicles
                                               .Include(ev => ev.CarCompany)
                                               .Include(ev => ev.VehicleCategory)
                                               .AsNoTracking();

            if (predicate != null) q = q.Where(predicate);

            return await q.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<ElectricVehicle> CreateAsync(ElectricVehicle vehicle)
        {
            _db.ElectricVehicles.Add(vehicle);
            await _db.SaveChangesAsync();
            return vehicle;
        }

        public async Task<ElectricVehicle> UpdateAsync(ElectricVehicle vehicle)
        {
            _db.ElectricVehicles.Update(vehicle);
            await _db.SaveChangesAsync();
            return vehicle;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var v = await _db.ElectricVehicles.FindAsync(id);
            if (v == null) return false;

            _db.ElectricVehicles.Remove(v);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
