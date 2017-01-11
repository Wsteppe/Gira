using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Gira.Data.Entities;
using Gira.Data.Enums;
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

        /// <summary>
        /// Quick seed method adding some basic test data to the application
        /// </summary>
        /// <param name="context"></param>
        private static void CreateRolesAndUsers(GiraDbContext context)
        {
            //No DI due to time
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            var db = new GiraUoW(context);

            var roles = new[] { "User", "Manager", "Dispatcher", "Solver", "Administrator" };

            foreach (var role in roles)
            {
                //check not even necessary because database is being dropped on model change.
                var roleExists = roleManager.RoleExists(role);
                if (!roleExists)
                    roleManager.Create(new IdentityRole { Name = role });
            }

            #region users
            var ted = new ApplicationUser
            {
                UserName = "Ted",
                Email = "Ted@mail.com",
                Surname = "Tedder",
                GivenName = "Ted",
            };
            userManager.Create(ted);
            userManager.AddToRole(ted.Id, "Manager");

            var bob = new ApplicationUser
            {
                UserName = "Bob",
                Email = "Bob@mail.com",
                Surname = "Bobber",
                GivenName = "Bob",
            };
            userManager.Create(bob);
            userManager.AddToRole(bob.Id, "Dispatcher");

            var jan = new ApplicationUser
            {
                UserName = "Jan",
                Email = "Jan@mail.com",
                Surname = "Janner",
                GivenName = "Jan",
            };
            userManager.Create(jan);
            userManager.AddToRole(jan.Id, "Solver");

            var louis = new ApplicationUser
            {
                UserName = "Louis",
                Email = "Louis@mail.com",
                Surname = "Loeder",
                GivenName = "Louis",
            };
            userManager.Create(louis);
            userManager.AddToRole(louis.Id, "Administrator");

            #endregion

            #region issues

            context.SaveChanges();

            #endregion
        }
    }
}