using MagicVilla.Data;
using MagicVilla.Models;
using MagicVilla.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla.Repository
{
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<Villa> Update(Villa enity)
        {
            enity.UpdatedDate = DateTime.Now;
             _db.Villas.Update(enity);
            await _db.SaveChangesAsync();
            return enity;
        }

       
    }
}
