using System;
using System.Threading.Tasks;
using Gira.Data.Entities;
using Gira.Data.Repositories.Instances;
using Gira.Data.Repositories.Interfaces;

namespace Gira.Data
{
    public sealed class GiraUoW : IGiraUoW, IDisposable
    {
        private GiraContext _context;

        private IBaseRepository<Issue> _issues;

        public GiraUoW(GiraContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            _context = context;
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public IBaseRepository<Issue> Issues => _issues ?? (_issues = new BaseRepository<Issue>(_context));

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