using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gira.Data;
using Gira.Data.Entities;
using Microsoft.AspNet.Identity;

namespace Gira.Controllers
{
    public class HomeController : Controller
    {
        private readonly GiraDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(GiraDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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
            var user = await _userManager.FindByIdAsync(User.Identity.GetUserId());
            var adminRole = await _db.Roles.FirstOrDefaultAsync(r => r.Name.Equals("Administrator"));
            if (user.Roles.Any(r => r.RoleId == adminRole.Id))
                await _userManager.RemoveFromRoleAsync(user.Id, "Administrator");
            else
            {
                await _userManager.AddToRoleAsync(user.Id, "Administrator");
            }
            await _db.SaveChangesAsync();

            return RedirectToAction("LogOut", "Account");
        }
    }
}