using System;
using Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using Interfaces;

namespace Implementations
{
    public class EfRepository<T> : IAsyncRepository<T> where T : BaseEntity
    {
        protected DbContext Context;

        public EfRepository(DbContext context)
        {
            Context = context;
        }

        public async Task Add(T entity)
        {
            await Context.Set<T>().AddAsync(entity);
            await Context.SaveChangesAsync();
        }

        public async Task<int> CountAll()
        {
            return await Context.Set<T>().CountAsync();
        }

        public async Task<int> CountWhere(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().CountAsync(predicate);
        }

        public async Task<T> FirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await Context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(int id)
        {
            return await Context.Set<T>().FindAsync(id);
        }

        public async Task<IEnumerable<T>> GetWhere(Expression<Func<T, bool>> predicate)
        {
            return await Context.Set<T>().Where(predicate).ToListAsync();
        }

        public Task Remove(T entity)
        {
            Context.Set<T>().Remove(entity);
            return Context.SaveChangesAsync();
        }

        public Task Update(T entity)
        {
            Context.Entry(entity).State = EntityState.Modified;
            return Context.SaveChangesAsync();
        }
    }
}
