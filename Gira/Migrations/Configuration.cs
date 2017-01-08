using Gira.Data;

namespace Gira.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Gira.Data.GiraDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(GiraDbContext context)
        {
            GiraInitializer.CreateRolesAndUsers(context);
        }
    }
}
