using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;
        public Repository(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public async Task Create(T enity)
        {
            await dbSet.AddRangeAsync(enity);
            await Save();
        }

        public async Task<T> Get(Expression<Func<T, bool>> filter = null, bool tracked = true)
        {
            IQueryable<T> query = dbSet;
            if (!tracked) query.AsNoTracking();
            if (filter != null) query = query.Where(filter);

            return await query.FirstOrDefaultAsync();


        }

        public async Task<List<T>> GetAll(Expression<Func<T, bool>>? filter = null)
        {
            IQueryable<T> query = dbSet; ;

            if (filter != null)
            {
                query = query.Where(filter);
            }
            return await query.ToListAsync();
        }

        public async Task Remove(T enity)
        {
            dbSet.Remove(enity);
            await Save();

        }


        public async Task Save()
        {
            await _db.SaveChangesAsync();

        }
    }
}
