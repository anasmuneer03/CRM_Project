using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Notes
{
    public class NoteRequest
    {
        public string Content { get; set; } = string.Empty;
        public int? LeadId { get; set; }
        public int? CustomerId { get; set; }
        public int? OpportunityId { get; set; }
    }
}
