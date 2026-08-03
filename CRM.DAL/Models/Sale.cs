using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public class Sale
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public CurrancyEnum Currancy { get; set; } = CurrancyEnum.JOD;
        public DateTime SaleDate { get; set; } = DateTime.UtcNow;
        public int OpportunityId { get; set; }
        public Opportunity Opportunity { get; set; }
        public int CustomerId { get; set; }
        public Customer Customer { get; set; }
        public string? AssignedAgentId { get; set; }
        public ApplicationUser? AssignedAgent { get; set; }
        //public ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
