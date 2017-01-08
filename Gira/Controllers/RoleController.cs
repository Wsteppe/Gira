using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gira.Data;
using Microsoft.AspNet.Identity;

namespace Gira.Controllers
{
    public class RoleController : Controller
    {
        private readonly GiraDbContext _context;

        public RoleController(GiraDbContext context)
        {
            _context = context;
        }
        //top: select users, bottom see list of potential roles.

        // GET: Role
        public async Task<ActionResult> Index()
        {
            var roles = await _context.Roles.ToListAsync();
            return View(roles);
        }        
    }
}
