using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Response.CrmTasks
{
    public class CrmTaskResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; } = string.Empty;

        public DateTime DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public bool IsOverdue { get; set; } // computed: DueDate < now, and not Completed/Cancelled

        public TaskStatusEnum Status { get; set; }
        public string StatusDisplay { get; set; } = string.Empty;

        public TaskPriorityEnum Priority { get; set; }
        public string PriorityDisplay { get; set; } = string.Empty;

        public string? AssignedToId { get; set; }
        public string? AssignedToName { get; set; }

        public int? LeadId { get; set; }
        public int? CustomerId { get; set; }
        public int? OpportunityId { get; set; }

        public string? CreatedById { get; set; }
        public string CreatedByName { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
