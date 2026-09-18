using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Response.Opportunities
{
    public class OpportunityResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public decimal Amount { get; set; }
        public CurrancyEnum Currancy { get; set; }


        public int Probability { get; set; }
        public decimal WeightedValue { get; set; }

        public OpportunityStageEnum Stage { get; set; } 

        public DateTime? ExpectedClosedDate { get; set; }
        public DateTime? ActualClosedDate { get; set; }

        public string? LostReason { get; set; }
        public string? Description { get; set; }

        public string? AssignedAgentId { get; set; }
        public string? AssignedAgentName { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public int? SaleId { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
    }
}
