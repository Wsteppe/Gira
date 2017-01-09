using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Gira.Data.Repositories.Interfaces;

namespace Gira.Data.Repositories.Instances
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly DbContext DbContext;
        protected DbSet<T> Entities { get; }

        internal Repository(DbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));

            DbContext = context;
            Entities = context.Set<T>();
        }

        public void Add(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Entities.Add(entity);
        }

        public void Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            Entities.Attach(entity);
            DbContext.Entry(entity).State = EntityState.Modified;
        }

        public void Remove(T entity)
        {
            Entities.Attach(entity);
            Entities.Remove(entity);
        }

        public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.Where(predicate).ToListAsync();
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await Entities.ToListAsync();
        }

        public async Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate)
        {
            return await Entities.SingleOrDefaultAsync(predicate);
        }
    }
}