using System;
using System.ComponentModel.DataAnnotations.Schema;
using Gira.Data.Enums;
using Gira.Models;

namespace Gira.Data.Entities
{
    public class Issue : BaseEntity
    {
        public string Subject { get; set; }
        public string Description { get; set; }
        public PriorityCode PriorityCode { get; set; }
        public DateTime? Occurrence { get; set; }
        [ForeignKey("ResponsibleUserId")]
        public ApplicationUser ResponsibleUser { get; set; }
        public string ResponsibleUserId { get; set; }
        public DateTime? Registered { get; set; }

    }
}