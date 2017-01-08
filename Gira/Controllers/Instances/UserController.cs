using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Security;
using Gira.Data;
using Gira.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Controllers.Instances
{
    public class UserController : Controller
    {
        private readonly RoleController _roleController;
        private readonly GiraDbContext _context;
        public UserController(RoleController roleController, GiraDbContext context)
        {
            _roleController = roleController;
            _context = context;
        }

        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Index()
        {
            var store = new UserStore<ApplicationUser>(_context);
            var users = await _context.Users.ToListAsync();

            return View();
            }
    }
}