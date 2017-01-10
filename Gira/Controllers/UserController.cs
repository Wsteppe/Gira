using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Gira.Data;
using Gira.Data.Entities;

namespace Gira.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IGiraUoW _db;

        public UserController(IGiraUoW db)
        {
            _db = db;
        }

        // GET: User
        public async Task<ActionResult> Index()
        {
            return View(await _db.Users.GetAllAsync());
        }

        // GET: User/Details/5
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicationUser = await _db.Users.GetAsync(id);
            if (applicationUser == null)
                return HttpNotFound();

            return View(applicationUser);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            return View();
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var applicationUser = await _db.Users.GetAsync(id);

            if (applicationUser == null)
                return HttpNotFound();

            return View(applicationUser);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(ApplicationUser applicationUser)
        {
            if (applicationUser == null || !ModelState.IsValid) return View(applicationUser);
        
            _db.Users.Update(applicationUser);
            await _db.SaveAsync();
            return RedirectToAction("Index");
        }

        // GET: User/Delete/5
        public async Task<ActionResult> Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicationUser = await _db.Users.GetAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: User/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var applicationUser = await _db.Users.GetAsync(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            _db.Users.Remove(applicationUser);
            await _db.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
