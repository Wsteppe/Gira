using System.Data.Entity;
using Gira.Data.Entities;
using Gira.Migrations;
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
    }
}