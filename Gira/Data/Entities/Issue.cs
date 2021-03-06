﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Gira.Data.Enums;

namespace Gira.Data.Entities
{
    public class Issue : BaseEntity
    {
        [Required]
        public string Subject { get; set; }
        public string Description { get; set; }
        [DisplayName(@"Issue Priority")]
        public PriorityCode? PriorityCode { get; set; }
        [Required]
        [DisplayName(@"Current Status")]
        public IssueStatusCode IssueStatusCode { get; set; }
        public DateTime? Occurrence { get; set; }

        /// <summary>
        /// User currently responsible for the issue
        /// </summary>
        [ForeignKey("ResponsibleUserId")]
        [DisplayName(@"Responsible User")]
        public virtual ApplicationUser ResponsibleUser { get; set; }
        public string ResponsibleUserId { get; set; }

        /// <summary>
        /// Original submitter of the issue
        /// </summary>
        [ForeignKey("CreatorId")]
        public virtual ApplicationUser Creator { get; set; }
        public string CreatorId { get; set; }

        public DateTime? Registered { get; set; }

        public virtual ICollection<IssueHistory> Histories { get; set; }
    }
}