using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Invoices
{
    public class UpdateInvoiceStatusRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public InvoiceStatusEnum InvoiceStatus { get; set; }
    }
}
