using CRM.DAL.Data;
using CRM.DAL.Models;
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

        public async Task<List<T>> GetAllAsync(Expression<Func<T, bool>>? filter,
            string[]? includes = null, bool includeDeleted = false)
        {
            var query = BuildQuery(filter, includes, includeDeleted);
            return await query.ToListAsync();
        }

        public IQueryable<T> GetQueryable(Expression<Func<T, bool>>? filter, string[]? includes = null,
            bool includeDeleted = false)
        {
            return BuildQuery(filter, includes, includeDeleted);

        }

        public async Task<T?> GetOneAsync(Expression<Func<T, bool>> filter, string[]? includes = null,
            bool includeDeleted = false)
        {
            var query = BuildQuery(null, includes, includeDeleted);
            return await query.FirstOrDefaultAsync(filter);
        }
        public async Task<T> CreateAsync(T entity)
        {
            await _context.AddAsync(entity);
            return entity;
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public void DeleteRangeAsync(List<T> entities)
        {
            _context.RemoveRange(entities);
        }

        public void Update(T entity)
        {
            _context.Update(entity);
        }

        public void UpdateRangeAsync(List<T> entities)
        {
            _context.UpdateRange(entities);
        }
        private IQueryable<T> BuildQuery(Expression<Func<T,bool>>? filter,string[]? includes,
            bool includeDeleted = false)
        {
            IQueryable<T> query = _context.Set<T>().AsNoTracking();

            if (!includeDeleted && typeof(AuditableEntity).IsAssignableFrom(typeof(T)))
            {
                query = query.Where(e => EF.Property<EntityStatusEnum>(e, "EntityStatus") != EntityStatusEnum.InActive);
            }

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
