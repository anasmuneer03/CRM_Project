using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public enum LeadStatusEnum
    {
        New = 1,
        Contacted = 2,
        Qualified = 3,
        Lost = 4,
        Converted = 5
    }
    public enum LeadSourceEnum
    {
        Website = 1,
        Referral = 2,
        ColdCall = 3,
        SocialMedia = 4,
        Advertisement = 5,
        Other = 99
    }
    public class Lead 
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone {  get; set; }
        public string? CompanyName { get; set; }
        public LeadSourceEnum LeadSource { get; set; }
        public LeadStatusEnum LeadStatus { get; set; } = LeadStatusEnum.New;
        public string? LostReason { get; set; }
        public int? ConvertedToCustomerId { get; set; }
        public Customer? ConvertedToCustomer { get; set; }
        public DateTime? ConvertedAt { get; set; }
        public string? AssignedAgentId { get; set; }
        public ApplicationUser? AssignedAgent { get; set; }
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<CrmTask> CrmTasks { get; set; } = new List<CrmTask>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();


    }
}
