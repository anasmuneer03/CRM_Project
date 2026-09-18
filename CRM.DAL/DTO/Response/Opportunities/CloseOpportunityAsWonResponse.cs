using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Response.Opportunities
{
    public class CloseOpportunityAsWonResponse
    {
        public int SaleId { get; set; }
        public int OpportunityId { get; set; }
        public DateTime SaleDate { get; set; } 
    }
}
