using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public enum CustomerStatusEnum
    {
        Active = 1,
        InActive = 2,
        Churned = 3
    }
    public class Customer 
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string? CompanyName { get; set; }
        public string Address { get; set; }
        public CustomerStatusEnum CustomerStatus { get; set; }
        public int? ConvertedFromLeadId { get; set; }
        public Lead? ConvertedFromLead { get; set; }
        public string? AssignedAgentId { get; set; }
        public ApplicationUser? AssignedAgent { get; set; }
        public ICollection<Opportunity> Opportunities { get; set; } = new List<Opportunity>();
        public ICollection<Sale> Sales { get; set; } = new List<Sale>();
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<CrmTask> CrmTasks { get; set; } = new List<CrmTask>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>(); 
    }
}
