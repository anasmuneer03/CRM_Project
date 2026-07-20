using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Repository
{
    public interface IGenericRepository<T> where T: class
    {
        Task<List<T>> GetAllAsync(Expression<Func<T,bool>>? filter = null, string[]? includes = null);
        IQueryable<T> GetQueryable(Expression<Func<T, bool>>? filter = null, string[]? includes = null);
        Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, string[]? includes = null);
        Task<T> CreateAsync(T entity);
        void UpdateAsync(T entity);
        void UpdateRangeAsync(List<T> entities);
        void DeleteAsync(T entity);
        void DeleteRangeAsync(List<T> entities);
    }
}
