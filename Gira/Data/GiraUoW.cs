using System;
using System.Threading.Tasks;
using Gira.Data.Entities;
using Gira.Data.Repositories.Instances;
using Gira.Data.Repositories.Interfaces;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Data
{
    public sealed class GiraUoW : IGiraUoW, IDisposable
    {
        private GiraDbContext _context;

        private IEntityRepository<Issue> _issues;
        private IIdentityRepository<IdentityRole> _roles;
        private IIdentityRepository<ApplicationUser> _users;


        public GiraUoW(GiraDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IEntityRepository<Issue> Issues => _issues ?? (_issues = new EntityRepository<Issue>(_context));
        public IIdentityRepository<IdentityRole> Roles => _roles ?? (_roles = new RoleRepository(_context));
        public IIdentityRepository<ApplicationUser> Users => _users ?? (_users = new ApplicationUserRepository(_context));

        #region dispose
        public void Dispose()
        {
            Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (!disposing || _context == null) return;

            _context.Dispose();
            _context = null;
        }
#endregion
    }
}