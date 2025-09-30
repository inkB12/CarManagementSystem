using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace CarManagementSystem.DataAccess.Repositories.Services
{
    public class CarCompanyRepository : ICarCompanyRepository
    {
        private readonly CarManagementDbContext _db;

        public CarCompanyRepository(CarManagementDbContext db)
        {
            _db = db;
        }

        public async Task<CarCompany> GetByIdAsync(int id) =>
            await _db.CarCompanies.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<List<CarCompany>> GetAllAsync(Expression<Func<CarCompany, bool>>? predicate = null)
        {
            IQueryable<CarCompany> q = _db.CarCompanies.AsNoTracking();
            if (predicate != null) q = q.Where(predicate);
            return await q.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<CarCompany> CreateAsync(CarCompany company)
        {
            _db.CarCompanies.Add(company);
            await _db.SaveChangesAsync();
            return company;
        }

        public async Task<CarCompany> UpdateAsync(CarCompany company)
        {
            _db.CarCompanies.Update(company);
            await _db.SaveChangesAsync();
            return company;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var c = await _db.CarCompanies.FindAsync(id);
            if (c == null) return false;

            _db.CarCompanies.Remove(c);
            await _db.SaveChangesAsync();
            return true;
        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
