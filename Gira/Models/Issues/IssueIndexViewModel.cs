using System.Collections.Generic;
using Gira.Data.Entities;

namespace Gira.Models.Issues
{
    public class IssueIndexViewModel
    {
        public IEnumerable<Issue> CreatedIssues { get; set; }

        public IEnumerable<Issue> IssuesToDispatch { get; set; }

        public IEnumerable<Issue> ResponsibleIssues { get; set; }

        public IEnumerable<Issue> ManagedIssues { get; set; }
    }
}