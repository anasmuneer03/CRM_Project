using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public enum InvoiceStatusEnum
    {
        Draft = 1,          // created but not yet sent to the customer
        Sent = 2,           // sent, awaiting payment, not yet overdue
        PartiallyPaid = 3,  // some payments received, but total < invoice.Amount
        Paid = 4,           // fully covered by successful payments
        Overdue = 5,        // past DueDate with an outstanding balance remaining
        Cancelled = 6       // voided, no payment expected
    }
    public class Invoice
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public decimal Amount { get; set; }
        public InvoiceStatusEnum InvoiceStatus { get; set; } = InvoiceStatusEnum.Draft;
        public DateTime IssueDate { get; set; } = DateTime.UtcNow;
        public DateTime? DueDate { get; set; }
        public int SaleId { get; set; }
        public Sale Sale { get; set; }
        //payments: an invoice can be paid in full at once, or in multiple installments 
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
}
