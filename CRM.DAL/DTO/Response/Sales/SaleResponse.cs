using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Response.Sales
{
    public class SaleResponse
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public CurrancyEnum Currancy { get; set; } 
        public DateTime SaleDate { get; set; } 
        public int OpportunityId { get; set; }
        public string OpportunityTitle { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string? AssignedAgentId { get; set; }
        public string? AssignedAgentName { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public ICollection<Invoice>? Invoices { get; set; } 
    }
}
