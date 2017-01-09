using System.Data.Entity;
using System.Threading.Tasks;
using Gira.Data.Entities;
using Gira.Data.Repositories.Interfaces;

namespace Gira.Data.Repositories.Instances
{
    public class ApplicationUserRepository : Repository<ApplicationUser>, IIdentityRepository<ApplicationUser>
    {
        internal ApplicationUserRepository(DbContext context) : base(context)
        {
        }

        public async Task<ApplicationUser> GetAsync(string id)
        {
            return await Entities.FirstOrDefaultAsync(e => e.Id.Equals(id));
        }
    }
}