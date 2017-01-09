using System;
using System.Data.Entity;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gira.Business.Interfaces;
using Gira.Data;
using Gira.Data.Entities;
using Gira.Data.Enums;
using Gira.Models.Issues;
using Gira.Resources;
using Gira.Utilities;
using Microsoft.AspNet.Identity;

namespace Gira.Controllers
{
    public class IssueController : Controller
    {
        private readonly IGiraUoW _db;
        private readonly ITransitionService _transitionService;

        public IssueController(IGiraUoW db, ITransitionService transitionService)
        {
            _db = db;
            _transitionService = transitionService;
        }

        public async Task<ActionResult> Index()
        {
            //get userId
            var userId = User.Identity.GetUserId();

            var model = new IssueIndexViewModel
            {
                CreatedIssues = await _db.Issues.FindAsync(i => i.CreatorId == userId),
                ResponsibleIssues = await _db.Issues.FindAsync(i => i.ResponsibleUserId == userId),
                ManagedIssues = await _db.Issues.FindAsync(i => i.ManagerId == userId),
            };

            if (User.IsInRole(SecurityRoles.Dispatcher.ToString()))
                model.IssuesToDispatch = await _db.Issues.FindAsync(i => i.IssueStatusCode == IssueStatusCode.New); 

            return View(model);
        }

        public async Task<ActionResult> Edit(int? id)
        {
            if(id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //get issue
            var issue = await _db.Issues.GetAsync(id.Value);

            if(issue == null)
                return HttpNotFound();

            var possibleTransitions = _transitionService.GetTransitions(issue);

            var model = new IssueEditViewModel
            {
                Issue = issue,
                Transitions = possibleTransitions
            };

            return View(model);
        }

        // POST: User/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Issue issue)
        {
            if (issue != null && ModelState.IsValid)
            {
                _db.Issues.Update(issue);
                await _db.SaveAsync();
                return RedirectToAction("Index");
            }
            var transitions = _transitionService.GetTransitions(issue);
            var model = new IssueEditViewModel
            {
                Issue = issue,
                Transitions = transitions
            };
            return View(model);
        }

        public async Task<ActionResult> Transition(int? id, IssueTransition? transition)
        {
           
            if (id == null || transition == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //get issue
            var issue = await _db.Issues.GetAsync(id.Value);

            if (issue == null)
                return HttpNotFound();

            issue = _transitionService.Transition(issue, transition.Value);

            //todo provide visual feedback of transition

            return RedirectToAction("Index", "Issue");
        }

        /// <summary>
        /// Directs user to create page
        /// </summary>
        /// <returns></returns>
        public ActionResult Create()
        {
            var model = new Issue();
            return View(model);
        }

        /// <summary>
        /// adds user to database and redirects to index
        /// </summary>
        /// <param name="issue"></param>
        /// <returns></returns>
        public async Task<ActionResult> Add(Issue issue)
        {
            if(issue.Subject == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            issue.CreatorId = User.Identity.GetUserId();
            issue.Registered = DateTime.Now;
            issue.IssueStatusCode = IssueStatusCode.New;
            _db.Issues.Add(issue);
            await _db.SaveAsync();
            return RedirectToAction("Index", "Issue");
        }
    }
}