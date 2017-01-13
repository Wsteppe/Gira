using System.Collections.Generic;
using System.Web.Mvc;
using Gira.Data.Entities;
using Gira.Data.Enums;

namespace Gira.Models.Issues
{
    public class IssueTransitionViewModel
    {
        public Issue Issue { get; set; }

        public IssueTransition Transition { get; set; }

        //load only users who are by their role acceptable
        public IEnumerable<SelectListItem> Solvers { get; set; }

        public string Comment { get; set; }

        public string SolverId { get; set; }
    }
}