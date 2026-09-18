using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Opportunities
{
    public class OpportunityRequest
    {
        public string Title { get; set; }
        public decimal Amount { get; set; }
        [Range(0, 100)]
        public int Probability { get; set; } = 10;

        [JsonConverter(typeof(JsonStringEnumConverter)),
            EnumDataType(typeof(CustomerStatusEnum))]
        public CurrancyEnum Currancy { get; set; }
        public DateTime? ExpectedClosedDate { get; set; }
        public string? Description { get; set; }
        public string? AssignedAgentId { get; set; }
        public int CustomerId { get; set; }
    }
}
