using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Leads
{
    public class LeadRequest
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }
        public string? CompanyName { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LeadSourceEnum LeadSource { get; set; }
        public string? AssignedAgentId { get; set; }
    }
}
