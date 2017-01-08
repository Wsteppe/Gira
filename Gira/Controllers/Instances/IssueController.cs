using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Gira.Business;
using Gira.Business.Interfaces;
using Gira.Data;
using Gira.Data.Entities;
using Gira.Data.Enums;
using Gira.Models.Issues;
using Gira.Resources;
using Microsoft.AspNet.Identity;

namespace Gira.Controllers.Instances
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
                return RedirectToAction("Index", "Issue");

            //get issue
            var issue = await _db.Issues.GetAsync(id.Value);

            if(issue == null)
                throw new BusinessException(BusinessErrors.IssueInvalid);

            var possibleTransactions = _transitionService.GetTransitions(issue);

            var model = new IssueEditViewModel
            {
                Issue = issue,
                Transitions = possibleTransactions
            };

            return View(model);
        }

        public async Task<ActionResult> Transition(int? id, IssueTransition? transition)
        {
           
            if (transition == null)
                return RedirectToAction("Index", "Issue");

            //get issue
            var issue = await _db.Issues.GetAsync(id.Value);

            if (issue == null)
                throw new BusinessException(BusinessErrors.IssueInvalid);

            var possibleTransactions = _transitionService.GetTransitions(issue);

            var model = new IssueEditViewModel
            {
                Issue = issue,
                Transitions = possibleTransactions
            };

            return View(model);
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
                throw new BusinessException(BusinessErrors.IssueInvalid);
            issue.CreatorId = User.Identity.GetUserId();
            issue.Registered = DateTime.Now;
            issue.IssueStatusCode = IssueStatusCode.New;
            _db.Issues.Add(issue);
            await _db.SaveAsync();
            return RedirectToAction("Index", "Issue");
        }
    }
}