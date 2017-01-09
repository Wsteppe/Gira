using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Gira.Data.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        void Update(T entity);
        void Remove(T entity);

        /// <summary>
        /// Finds all entities matching the predicate
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetAllAsync();
        /// <summary>
        /// finds first entity matching the predicate (or null)
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task<T> SingleOrDefaultAsync(Expression<Func<T, bool>> predicate);

    }
}
