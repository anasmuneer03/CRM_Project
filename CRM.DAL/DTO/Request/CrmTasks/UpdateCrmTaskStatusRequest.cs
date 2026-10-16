using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.CrmTasks
{
    public class UpdateCrmTaskStatusRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TaskStatusEnum Status { get; set; }
    }
}
