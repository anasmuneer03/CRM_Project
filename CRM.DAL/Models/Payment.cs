using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DAL.Models
{
    public enum PaymentMethodEnum
    {
        Cash = 1,
        CreditCard = 2,
        BankTransfer = 3,
        Cheque = 4,
        Other = 99
    }
    public enum PaymentStatusEnum
    {
        Pending = 1,
        Completed = 2,
        Failed = 3,
        Refunded = 4
    }
    public class Payment
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;
        public PaymentMethodEnum PaymentMethod { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Pending;
        public int InvoiceId { get; set; }
        public Invoice Invoice { get; set; }
    }
}
