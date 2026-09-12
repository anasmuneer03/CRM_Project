using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Response.Leads
{
    public class ConvertToCustomerResponse
    {
        public int CustomerId { get; set; }
        public int LeadId { get; set; }
        public DateTime ConvertedAt { get; set; }
    }
}
