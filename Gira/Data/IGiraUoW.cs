using System.Threading.Tasks;
using Gira.Data.Entities;
using Gira.Data.Repositories.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Data
{
    public interface IGiraUoW
    {
        Task SaveAsync();

        IEntityRepository<Issue> Issues { get; }
        IIdentityRepository<IdentityRole> Roles { get; }
        IIdentityRepository<ApplicationUser> Users { get; }
    }
}
