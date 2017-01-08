using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gira.Data.Enums;
using Gira.Models;

namespace Gira.Data.Entities
{
    public class Issue : BaseEntity
    {
        [Required]
        public string Subject { get; set; }
        public string Description { get; set; }
        public PriorityCode? PriorityCode { get; set; }
        [Required]
        public IssueStatusCode IssueStatusCode { get; set; }
        public DateTime? Occurrence { get; set; }

        /// <summary>
        /// User currently responsible for the issue
        /// </summary>
        [ForeignKey("ResponsibleUserId")]
        public ApplicationUser ResponsibleUser { get; set; }
        public string ResponsibleUserId { get; set; }

        /// <summary>
        /// Original submitter of the issue
        /// </summary>
        [ForeignKey("CreatorId")]
        public ApplicationUser Creator { get; set; }
        public string CreatorId { get; set; }

        /// <summary>
        /// Original submitter of the issue
        /// </summary>
        [ForeignKey("ManagerId")]
        public ApplicationUser Manager { get; set; }
        public string ManagerId { get; set; }

        public DateTime? Registered { get; set; }
    }
}