using System.Threading.Tasks;
using System.Web.Mvc;
using Gira.Data;

namespace Gira.Controllers
{
    public class RoleController : Controller
    {
        private readonly IGiraUoW _db;

        public RoleController(IGiraUoW db)
        {
            _db = db;
        }
        //top: select users, bottom see list of potential roles.

        // GET: Role
        public async Task<ActionResult> Index()
        {
            var roles = await _db.Roles.GetAllAsync();
            return View(roles);
        }        
    }
}
