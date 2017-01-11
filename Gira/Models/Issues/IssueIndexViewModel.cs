using System.Collections.Generic;
using Gira.Data.Entities;

namespace Gira.Models.Issues
{
    public class IssueIndexViewModel
    {
        /// <summary>
        /// All issues in the application, only visible as admin.
        /// </summary>
        public IEnumerable<Issue> AllIssues { get; set; }
        public IEnumerable<Issue> CreatedIssues { get; set; }

        public IEnumerable<Issue> IssuesToDispatch { get; set; }

        public IEnumerable<Issue> ResponsibleIssues { get; set; }

        public IEnumerable<Issue> ManagedIssues { get; set; }
    }
}