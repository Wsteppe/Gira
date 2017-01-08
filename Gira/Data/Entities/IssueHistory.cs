using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gira.Data.Enums;
using Microsoft.Ajax.Utilities;

namespace Gira.Data.Entities
{
    public class IssueHistory : Issue
    {
        private IssueStatusCode Status;
        private DateTime StatusUpDate;
        protected List<Issue> History = new List<Issue>();

        public IssueHistory(IssueStatusCode status, DateTime statusUpdate)
        {
           History.Add(new IssueHistory(status, statusUpdate) {Status = status, StatusUpDate = statusUpdate});

        }

        public List<Issue> GetIssueHistoy()
        {
            return History;
        }
    }


}