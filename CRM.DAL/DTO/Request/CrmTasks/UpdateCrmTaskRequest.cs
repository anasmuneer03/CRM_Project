using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.CrmTasks
{
    public class UpdateCrmTaskRequest
    {
        public string? Title { get; set; }

        public string? Description { get; set; }
        public DateTime? DueDate { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskPriorityEnum? Priority { get; set; }
    }
}
