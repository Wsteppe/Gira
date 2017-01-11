using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gira.Data;
using Gira.Data.Entities;
using Gira.Data.Enums;
using Microsoft.AspNet.Identity;

namespace Gira.Controllers
{
    public class HomeController : Controller
    {
        private readonly IGiraUoW _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public HomeController(IGiraUoW db, UserManager<ApplicationUser> userManager)
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
            var adminRole = await _db.Roles.SingleOrDefaultAsync(r => r.Name.Equals("Administrator"));
            if (user.Roles.Any(r => r.RoleId == adminRole.Id))
                await _userManager.RemoveFromRoleAsync(user.Id, "Administrator");
            else
            {
                await _userManager.AddToRoleAsync(user.Id, "Administrator");
            }

            await GenerateDummyIssues();

            return RedirectToAction("LogOut", "Account");
        }

        /// <summary>
        /// Generates dummy issues for our dummy admin.
        /// </summary>
        /// <returns></returns>
        public async Task GenerateDummyIssues()
        {
            //if dummy method was not called before
            if (await _db.Issues.SingleOrDefaultAsync(i => i.Subject.ToLower().Equals("epicTestIssue1")) == null)
            {
                var ted = await _db.Users.SingleOrDefaultAsync(u => u.UserName.Equals("Ted"));
                var bob = await _db.Users.SingleOrDefaultAsync(u => u.UserName.Equals("Bob"));
                var jan = await _db.Users.SingleOrDefaultAsync(u => u.UserName.Equals("Jan"));
                var louis = await _db.Users.SingleOrDefaultAsync(u => u.UserName.Equals("Louis"));

                var issueList = new List<Issue>
                {
                    new Issue
                    {
                        Creator = ted,
                        Subject = "epicTestIssue1",
                        Description = "Issue1",
                        IssueStatusCode = IssueStatusCode.New,
                        PriorityCode = PriorityCode.immediate,
                        Occurrence = DateTime.Now.AddDays(-10),
                        Registered = DateTime.Now
                    },
                    new Issue
                    {
                        Creator = bob,
                        Subject = "Issue2",
                        Description = "Issue2",
                        IssueStatusCode = IssueStatusCode.Closed,
                        PriorityCode = PriorityCode.low,
                        Occurrence = DateTime.Now.AddDays(-5),
                        Registered = DateTime.Now
                    },
                    new Issue
                    {
                        Creator = louis,
                        Subject = "Issue3",
                        Description = "Issue3",
                        IssueStatusCode = IssueStatusCode.Enquiring,
                        PriorityCode = PriorityCode.low,
                        Occurrence = DateTime.Now.AddDays(-5),
                        Registered = DateTime.Now,
                        ResponsibleUser = louis
                    },
                    new Issue
                    {
                        Creator = louis,
                        Subject = "Issue4",
                        Description = "Issue4",
                        IssueStatusCode = IssueStatusCode.Processing,
                        PriorityCode = PriorityCode.low,
                        Occurrence = DateTime.Now.AddDays(-15),
                        Registered = DateTime.Now,
                        ResponsibleUser = jan
                    }
                };

                foreach (var issue in issueList)
                {
                    _db.Issues.Add(issue);
                }

                await _db.SaveAsync();
            }
        }
    }
}