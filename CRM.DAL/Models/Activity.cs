using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public enum ActivityTypeEnum
    {
        Call = 1,
        Email = 2,
        Meeting = 3,
        SiteVisit = 4,
        WhatsApp = 5,
        Other = 99
    }
    public class Activity
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string? Description { get; set; }
        public ActivityTypeEnum Type {  get; set; }
        public DateTime ActivityDate { get; set; } = DateTime.UtcNow;
        // Relevant for Call/Meeting types - nullable since it doesn't apply to Email, etc.
        public int? DurationMinutes { get; set; }
        public int? LeadId { get; set; }
        public Lead? Lead { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? OpportunityId { get; set; }
        public Opportunity? Opportunity { get; set; }
    }
}
