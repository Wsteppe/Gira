using System;
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
using Microsoft.AspNet.Identity.EntityFramework;

namespace Gira.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : Controller
    {
        private readonly IGiraUoW _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(IGiraUoW db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
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

            var users = await _db.Users.GetAllAsync();

            var model = new UserCreateViewModel()
            {
                Managers = selectList,
                Users = users
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<PartialViewResult> Create(UserEditViewModel model)
        {
            if (model?.User == null || !ModelState.IsValid)
                throw new Exception(BusinessErrors.UserInvalid);

            _db.Users.Add(model.User);
            await _db.SaveAsync();

            var users = await _db.Users.GetAllAsync();

            return PartialView("_UserAdminListPartial", users);
        }

        // GET: User/Edit/5
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, BusinessErrors.UserInvalid);

            var applicationUser = await _db.Users.GetAsync(id);

            if (applicationUser == null)
                return HttpNotFound();

            //get roles
            var userRoles = await _db.Roles.FindAsync(r => r.Users.Any(u => u.UserId == id));
            var userRolesList = userRoles as IList<IdentityRole> ?? userRoles.ToList();

            var dbRoles = await _db.Roles.GetAllAsync();

            var selectRoles = new List<SelectListItem>();

            //make roles into selectlist
            foreach (var role in dbRoles)
            {
                var roleSelect = new SelectListItem
                {
                    Value = role.Id,
                    Text = role.Name,
                    Selected = userRolesList.Any(r => r.Id == role.Id)
                };
                selectRoles.Add(roleSelect);
            }

            //get managers
            var managers = await getManagers();

            if (managers == null)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, BusinessErrors.NoManagers);

            var model = new UserEditViewModel
            {
                User = applicationUser,
                Managers = managers,
                Roles = selectRoles,
                UserRoles = userRolesList
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

        [HttpPost]
        public async Task<PartialViewResult> AddRoles(UserEditViewModel model)
        {
            if (model?.User?.Id == null || model.Roles == null)
                throw new Exception(BusinessErrors.UserInvalid);

            var user = model.User;

            foreach (var role in model.Roles)
            {
                var isInRole = await _userManager.IsInRoleAsync(user.Id, role.Text);

                if (role.Selected && !isInRole)
                {
                    await _userManager.AddToRoleAsync(user.Id, role.Text);
                }
                else if (!role.Selected && isInRole)
                {
                    await _userManager.RemoveFromRoleAsync(user.Id, role.Text);
                }
            }

            var roles = await _db.Roles.FindAsync(r => r.Users.Any(u => u.UserId == user.Id));

            return PartialView("_RoleListPartial", roles);
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
