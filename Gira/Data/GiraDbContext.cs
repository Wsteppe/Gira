using Gira.Data.Entities;
using Gira.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Data
{
    public class GiraDbContext : IdentityDbContext<ApplicationUser>
    {
        public GiraDbContext()
            : base("GiraDbContext", throwIfV1Schema: false)
        {
        }

        public static GiraDbContext Create()
        {
            return new GiraDbContext();
        }

        public System.Data.Entity.DbSet<Gira.Data.Entities.Issue> Issues { get; set; }
    }
}