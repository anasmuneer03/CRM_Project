using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Response.Notes
{
    public class NoteResponse
    {
        public int Id { get; set; }
        public string Content { get; set; }

        public int? LeadId { get; set; }
        public int? CustomerId { get; set; }
        public int? OpportunityId { get; set; }

        public string? CreatedById { get; set; }
        public string CreatedByName { get; set; } = string.Empty;

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
