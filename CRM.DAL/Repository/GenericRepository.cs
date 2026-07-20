using CRM.DAL.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly ApplicationDbContext _context;

        public GenericRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter, string[]? includes = null)
        {
            var query = BuildQuery(filter, includes);
            return await query.ToListAsync();
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? filter, string[]? includes = null)
        {
            return BuildQuery(filter, includes);

        }

        public async Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, string[]? includes = null)
        {
            var query = BuildQuery(null, includes);
            return await query.FirstOrDefaultAsync(filter);
        }
        public async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            return entity;
        }

        public void DeleteAsync(T entity)
        {
            _context.Remove(entity);
        }

        public void DeleteRangeAsync(List<T> entities)
        {
            _context.RemoveRange(entities);
        }

        public void UpdateAsync(T entity)
        {
            _context.Update(entity);
        }

        public void UpdateRangeAsync(List<T> entities)
        {
            _context.UpdateRange(entities);
        }
        private IQueryable<T> BuildQuery(Expression<Func<T,bool>>? filter,string[]? includes)
        {
            var query = _context.Set<T>().AsNoTracking();
            if (filter != null)
                query = query.Where(filter);
          
            if(includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return query;
        }
    }
}
