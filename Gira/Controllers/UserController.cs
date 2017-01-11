using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Gira.Data;
using Gira.Data.Entities;
using Gira.Models.User;
using Gira.Resources;
using Microsoft.AspNet.Identity;

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
            var model = await _db.Users.GetAllAsync();
            return View(model);
        }

        [Authorize(Roles = "Administrator,Solver,Dispatcher,Manager")]
        public async Task<ActionResult> Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, BusinessErrors.UserInvalid);
            }
            var user = await _db.Users.GetAsync(id);
            if (user == null)
                return HttpNotFound();

            var model = new UserDetailViewModel
            {
                User = user
            };

            if (!User.IsInRole("Administrator") && !User.IsInRole("Manager")) return View(model);

            //add issues to detail view if user is manager
            var userId = User.Identity.GetUserId();
            model.Issues = await
                _db.Issues.FindAsync(i => i.ResponsibleUser.ManagerId == userId || i.Creator.ManagerId == userId);

            return View(model);
        }

        public async Task<List<SelectListItem>> getManagers()
        {
            var managerRole = await _db.Roles.SingleOrDefaultAsync(r => r.Name.ToLower().Equals("manager"));
            if (managerRole == null)
                return null;

            var managers = await _db.Users.FindAsync(u => u.Roles.Any(r => r.RoleId.Equals(managerRole.Id)));

            var list = managers.Select(manager => new SelectListItem
            {
                Text = manager.UserName,
                Value = manager.ManagerId
            }).ToList();

            list.Add(new SelectListItem
            {
                Text = "",
                Value = null
            });
            return list;

        }

        // GET: User/Create
        public async Task<ActionResult> Create()
        {
            var selectList = await getManagers();

            if(selectList == null)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, BusinessErrors.NoManagers);

            var model = new UserEditViewModel
            {
                Managers = selectList
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(UserEditViewModel model)
        {
            if (model?.User == null || !ModelState.IsValid) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _db.Users.Add(model.User);
            await _db.SaveAsync();

            return RedirectToAction("Index");
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            var applicationUser = await _db.Users.GetAsync(id);

            if (applicationUser == null)
                return HttpNotFound();

            var selectList = await getManagers();

            if (selectList == null)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, BusinessErrors.NoManagers);

            var model = new UserEditViewModel
            {
                User = applicationUser,
                Managers = selectList
            };

            return View(model);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(UserEditViewModel model)
        {
            if (model?.User == null || !ModelState.IsValid)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _db.Users.Update(model.User);
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
