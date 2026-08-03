using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public enum OpportunityStageEnum
    {
        Prospecting = 1,
        Qualification = 2,
        ProposalSent = 3,
        Negotiation = 4,
        ClosedWon = 5,
        ClosedLost = 6
    }
    public enum CurrancyEnum
    {
        JOD = 1,
        USD = 2
    }
    public class Opportunity
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        [Range(0,100)]
        public int Probability { get; set; }
        public CurrancyEnum Currancy { get; set; } = CurrancyEnum.JOD;
        [NotMapped]
        public decimal WeightedValue => Amount * Probability / 100m;
        public OpportunityStageEnum Stage { get; set; } = OpportunityStageEnum.Prospecting;
        public DateTime? ExpectedClosedDate { get; set; }
        public DateTime? ActualClosedDate { get; set; }
        public string? LostReason { get; set; }
        public string? Description { get; set; }
        public string? AssignedAgentId { get; set; }
        public ApplicationUser? AssignedAgent { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public Sale? Sale { get; set; } // resulting sale, once won
        public ICollection<Note> Notes { get; set; } = new List<Note>();
        public ICollection<Activity> Activities { get; set; } = new List<Activity>();
        public ICollection<CrmTask> CrmTasks { get; set; } = new List<CrmTask>();
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();

    }
}
