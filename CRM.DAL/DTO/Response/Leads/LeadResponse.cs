using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Response.Leads
{
    public class LeadResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Phone { get; set; }
        public string? CompanyName { get; set; }
        public LeadSourceEnum LeadSource { get; set; }
        public LeadStatusEnum LeadStatus { get; set; }
        public string? LostReason { get; set; }
        public string? AssignedAgentId { get; set; }
        public string? AssignedAgentName { get; set; }
        public int? ConvertedToCustomerId { get; set; }
        public DateTime? ConvertedAt { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
