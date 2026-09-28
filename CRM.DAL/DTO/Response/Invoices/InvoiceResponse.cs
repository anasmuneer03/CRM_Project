using CRM.DAL.DTO.Response.Payments;
using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.DTO.Response.Invoices
{
    public class InvoiceResponse
    {
        public int Id { get; set; }

        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public CurrancyEnum Currancy { get; set; } 
        public InvoiceStatusEnum InvoiceStatus { get; set; }

        public DateTime IssueDate { get; set; } 
        public DateTime? DueDate { get; set; }
        public int SaleId { get; set; }
        public string? Notes { get; set; }

        public decimal TotalPaid { get; set; }
        public decimal OutstandingBalance { get; set; }

        public List<PaymentResponse> Payments { get; set; } = new();

        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }

    }
}
