using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.CrmTasks
{
    public class CrmTaskRequest
    {
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }

        public TaskPriorityEnum? Priority { get; set; } 

        public string? AssignedToId { get; set; }
        public int? LeadId { get; set; }
        public int? CustomerId { get; set; }
        public int? OpportunityId { get; set; }
    }
}
