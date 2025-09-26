using CarManagementSystem.BusinessObjects;
using CarManagementSystem.DataAccess.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;

namespace CarManagementSystem.DataAccess.Repositories.Services
{
    public class UserRepository : IUserRepository
    {
        private readonly CarManagementDbContext _db;

        public UserRepository(CarManagementDbContext db)
        {
            _db = db;
        }

        public async Task<User> GetByIdAsync(int id) =>
            await _db.Users.FirstOrDefaultAsync(x => x.Id == id);

        public async Task<User> GetByEmailAsync(string email) =>
            await _db.Users.FirstOrDefaultAsync(x => x.Email == email);

        public async Task<List<User>> GetAllAsync(Expression<Func<User, bool>>? predicate = null)
        {
            IQueryable<User> q = _db.Users.AsNoTracking();
            if (predicate != null) q = q.Where(predicate);
            return await q.OrderBy(x => x.Id).ToListAsync();
        }

        public async Task<User> CreateAsync(User user)
        {
            _db.Users.Add(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<User> UpdateAsync(User user)
        {
            _db.Users.Update(user);
            await _db.SaveChangesAsync();
            return user;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var u = await _db.Users.FindAsync(id);
            if (u == null) return false;

            _db.Users.Remove(u);
            await _db.SaveChangesAsync();
            return true;

        }

        public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
    }
}
