using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Customers
{
    public class UpdateCustomerStatusRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter)),
            EnumDataType(typeof(CustomerStatusEnum))]
        public CustomerStatusEnum CustomerStatus { get; set; }
        public string? ChurnReason { get; set; }
    }
}
