using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public enum TaskStatusEnum
    {
        Pending = 1,
        InProgress = 2,
        Completed = 3,
        Cancelled = 4
    }
    public enum TaskPriorityEnum
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
    public class CrmTask
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime? CompletedAt { get; set; }
        public TaskStatusEnum Status { get; set; } = TaskStatusEnum.Pending;
        public TaskPriorityEnum Priority { get; set; } = TaskPriorityEnum.Medium;
        public string? AssignedToId { get; set; }
        public ApplicationUser? AssignedTo {  get; set; }
        public int? LeadId { get; set; }
        public Lead? Lead {  get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? OpportunityId { get; set; }
        public Opportunity? Opportunity { get; set; } 

    }
}
