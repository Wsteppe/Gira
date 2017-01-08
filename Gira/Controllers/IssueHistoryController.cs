using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Gira.Business;
using Gira.Data;
using Gira.Data.Entities;

namespace Gira.Controllers
{
    public class IssueHistoryController : Controller
    {
        private readonly TransitionService _transitionService;
        private readonly GiraDbContext _context;


        public IssueHistoryController(TransitionService transitionService, GiraDbContext context)
        {
            _context = context;
            _transitionService = transitionService;
        }

        
        // GET: IssueHistory
        public ActionResult Index()
        {

            return View();
        }

        
    }

}