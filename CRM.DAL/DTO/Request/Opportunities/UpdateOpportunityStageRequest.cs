using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Opportunities
{
    public class UpdateOpportunityStageRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public OpportunityStageEnum Stage { get; set; }
        public string? LostReason { get; set; }
    }
}
