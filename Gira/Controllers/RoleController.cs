using System.Threading.Tasks;
using System.Web.Mvc;
using Gira.Data;
using Gira.Models;

namespace Gira.Controllers
{
    [Authorize(Roles = "Administrator")]
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
            var users = await _db.Users.GetAllAsync();

            var model = new RoleListViewModel
            {
                Roles = roles,
                Users = users
            };

            return View(model);
        }          
    }
}
