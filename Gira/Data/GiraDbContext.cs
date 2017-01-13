using Gira.Data.Entities;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Data
{
    //no need to register dbsets, as they are all loaded together with the applciationUser association.
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

        public System.Data.Entity.DbSet<Gira.Data.Entities.IssueHistory> IssueHistories { get; set; }

        public System.Data.Entity.DbSet<Gira.Data.Entities.Issue> Issues { get; set; }
    }
}