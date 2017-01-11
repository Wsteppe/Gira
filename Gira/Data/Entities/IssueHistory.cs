using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Gira.Data.Enums;
using Microsoft.Ajax.Utilities;

namespace Gira.Data.Entities
{
    public class IssueHistory : BaseEntity
    {
        private Enum IssueStatusCode;
        private DateTime StatusUpDate;
        private BaseEntity IssueId;
        private SecurityRoles Role;


        public IssueHistory(Enum issueStatusCode, DateTime statusUpDate, int issueId, SecurityRoles role)
        {
            this.IssueStatusCode = issueStatusCode;
            this.StatusUpDate = statusUpDate;
            this.IssueId = BaseEntity(issueId);
            this.Role = role;
        }

    }


}