using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Request.Invoices
{
    public class RecordPaymentRequest
    {
        public decimal Amount { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentMethodEnum PaymentMethod { get; set; }

        public DateTime? PaymentDate { get; set; }

        // Defaults to Completed (e.g. cash paid in hand). Override to Pending for something
        // like a bank transfer still awaiting confirmation.
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PaymentStatusEnum Status { get; set; } = PaymentStatusEnum.Completed;
        public string? ReferenceNumber { get; set; }
        public string? Notes { get; set; }
    }
}
