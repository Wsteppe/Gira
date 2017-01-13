using System;
using Gira.Data.Enums;

namespace Gira.Data.Entities
{
    public class IssueHistory : BaseEntity
    {
        public int? IssueId { get; set; }
        public Issue Issue { get; set; }

        public IssueStatusCode Status { get; set; }

        public DateTime? CreatedOn { get; set; }

        public string UserId { get; set; }
        public virtual ApplicationUser User { get; set; }

        public string Comment { get; set; }
    }
}