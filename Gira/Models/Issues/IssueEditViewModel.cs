using System.Collections.Generic;
using Gira.Data.Entities;
using Gira.Data.Enums;

namespace Gira.Models.Issues
{
    public class IssueEditViewModel
    {
        public Issue Issue { get; set; }

        public IEnumerable<IssueTransition> Transitions { get; set; }
    }
}