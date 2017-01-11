using System.Collections.Generic;
using Gira.Data.Entities;
using Gira.Data.Enums;

namespace Gira.Models.Issues
{
    public class IssueTransitionViewModel
    {
        public Issue Issue { get; set; }

        public IssueTransition Transition { get; set; }

        //load only users who are by their role acceptable
        public IEnumerable<ApplicationUser> Solvers { get; set; }
    }
}