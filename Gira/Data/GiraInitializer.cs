using System.Data.Entity;
using System.Threading.Tasks;
using Gira.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Data
{
    public class GiraInitializer : DropCreateDatabaseIfModelChanges<GiraDbContext>
    {
        protected override void Seed(GiraDbContext context)
        {
            CreateRolesAndUsers(context);
        }

        public static void CreateRolesAndUsers(DbContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var roles = new[] { "User", "Manager", "Dispatcher", "Solver", "Administrator" };

            foreach (var role in roles)
            {
                var roleExists = roleManager.RoleExists(role);
                if (!roleExists)
                    roleManager.Create(new IdentityRole { Name = role });
            }
        }
    }
}