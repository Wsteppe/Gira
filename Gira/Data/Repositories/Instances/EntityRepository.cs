using System.Data.Entity;
using System.Threading.Tasks;
using Gira.Data.Entities;
using Gira.Data.Repositories.Interfaces;

namespace Gira.Data.Repositories.Instances
{
    public class EntityRepository<T> : Repository<T>, IEntityRepository<T> where T : BaseEntity
    {
        internal EntityRepository(DbContext context) : base(context)
        {         
        }

        public async Task<T> GetAsync(int id)
        {
            return await Entities.FirstOrDefaultAsync(e => e.Id == id);
        }
    }
}