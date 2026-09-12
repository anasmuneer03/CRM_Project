using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Leads
{
    public class UpdateStatusRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LeadStatusEnum LeadStatus { get; set; }
        public string? LostReason { get; set; }
    }
}
