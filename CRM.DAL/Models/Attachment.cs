using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public class Attachment
    {
        public int Id { get; set; }
        public string FileName { get; set; }
        public string StoragePath { get; set; }
        public string? Description { get; set; }
        public int? LeadId { get; set; }
        public Lead? Lead { get; set; }
        public int? CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? OpportunityId {  get; set; }
        public Opportunity? Opportunity { get; set; }
    }
}
