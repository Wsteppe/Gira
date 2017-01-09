using System.Data.Entity;
using System.Threading.Tasks;
using Gira.Data.Repositories.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Data.Repositories.Instances
{
    public class RoleRepository : Repository<IdentityRole>, IIdentityRepository<IdentityRole>
    {
        internal RoleRepository(DbContext context) : base(context)
        {
        }

        public async Task<IdentityRole> GetAsync(string id)
        {
            return await Entities.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }
    }
}