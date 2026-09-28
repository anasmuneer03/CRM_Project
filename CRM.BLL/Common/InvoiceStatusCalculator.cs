using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Common
{
    public static class InvoiceStatusCalculator
    {
        public static InvoiceStatusEnum ComputeStatus(Invoice invoice)
        {
            if (invoice.InvoiceStatus == InvoiceStatusEnum.Cancelled)
                return InvoiceStatusEnum.Cancelled;

            var totalPaid = invoice.Payments
                .Where(p => p.PaymentStatus == PaymentStatusEnum.Completed)
                .Sum(p => p.Amount);

            if (totalPaid >= invoice.Amount)
                return InvoiceStatusEnum.Paid;

            if (totalPaid > 0)
                return InvoiceStatusEnum.PartiallyPaid;

            if (invoice.DueDate.HasValue && invoice.DueDate.Value < DateTime.UtcNow)
                return InvoiceStatusEnum.Overdue;

            return invoice.InvoiceStatus == InvoiceStatusEnum.Draft ? InvoiceStatusEnum.Draft : InvoiceStatusEnum.Sent;
        }


        public static decimal GetCommittedAmount(Invoice invoice) =>
        invoice.Payments
            .Where(p => p.EntityStatus != EntityStatusEnum.InActive
                        && (p.PaymentStatus is PaymentStatusEnum.Completed or PaymentStatusEnum.Pending))
            .Sum(p => p.Amount);
    }
}
