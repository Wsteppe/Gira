using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Gira.Data;
using Gira.Data.Entities;
using Gira.Data.Enums;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Controllers
{
    public class HomeController : Controller
    {
        private readonly GiraDbContext _db;

        public HomeController(GiraDbContext db)
        {
            _db = db;
        }

        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Triggers admin status for testing purposes to make life easier for Henk.
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> TriggerAdmin()
        {
            var userStore = new UserStore<ApplicationUser>(_db);
            var userManager = new UserManager<ApplicationUser>(userStore);
            var user = await userManager.FindByIdAsync(User.Identity.GetUserId());
            var adminRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name.Equals("Administrator"));
            if (user.Roles.Any(r => r.RoleId == adminRole.Id))
                await userManager.RemoveFromRoleAsync(user.Id, "Administrator");
            else
            {
                await userManager.AddToRoleAsync(user.Id, "Administrator");
            }
            await _db.SaveChangesAsync();

            return RedirectToAction("LogOut", "Account");
        }
    }
}