using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Response.Customers
{
    public class CustomerResponse
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? CompanyName { get; set; }
        public string Address { get; set; } = string.Empty;
        public CustomerStatusEnum CustomerStatus { get; set; }
        public int? ConvertedFromLeadId { get; set; }
        public string? AssignedAgentId { get; set; }
        public string? AssignedAgentName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ICollection<Opportunity> Opportunities { get; set; }
    }
}
