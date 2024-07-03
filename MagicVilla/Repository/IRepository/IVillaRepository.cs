using MagicVilla.Models;
using System.Linq.Expressions;

namespace MagicVilla.Repository.IRepository
{
    public interface IVillaRepository : IRepository<Villa>
    {
       
        Task<Villa> Update(Villa enity);
    }
}
