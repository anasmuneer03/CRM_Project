using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Invoices;
using CRM.DAL.DTO.Response.Invoices;
using CRM.DAL.Models;
using CRM.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Invoices
{
    public class InvoiceService :IInvoiceService
    {
        private readonly IUnitOfWork _uow;
        private static readonly string[] Includes = { nameof(Invoice.Sale), nameof(Invoice.Payments) }; 
        public InvoiceService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<InvoiceResponse>> GetAllInvoices()
        {
            var invoices = await _uow.Repository<Invoice>().GetAllAsync(includes: Includes);
            return invoices.Adapt<List<InvoiceResponse>>();
        }

        public async Task<InvoiceResponse?> GetInvoice(Expression<Func<Invoice, bool>> filter)
        {
            var invoice = await _uow.Repository<Invoice>().GetOneAsync(filter,
                includes : Includes);
            return invoice?.Adapt<InvoiceResponse>();
        }
        public async Task<ServiceResult<InvoiceResponse>> CreateInvoice(InvoiceRequest request)
        {
            var sale = await _uow.Repository<Sale>().GetOneAsync(filter:
                s => s.Id == request.SaleId, includes: new string[] { nameof(Sale.Invoices)});
            if (sale is null)
                return ServiceResult<InvoiceResponse>.NotFound("Sale not found");

            var duplicateNumber = await _uow.Repository<Invoice>().GetOneAsync(
                filter: i => i.InvoiceNumber == request.InvoiceNumber);

            if (duplicateNumber != null)
                return ServiceResult<InvoiceResponse>.Fail("An invoice with this number already exists");

            var activeInvoicesTotal = sale.Invoices
            .Where(i => i.EntityStatus != EntityStatusEnum.InActive && i.InvoiceStatus != InvoiceStatusEnum.Cancelled)
            .Sum(i => i.Amount);

            if (activeInvoicesTotal + request.Amount > sale.Amount)
            {
                var remaining = sale.Amount - activeInvoicesTotal;
                return ServiceResult<InvoiceResponse>.Fail(
                    $"This invoice would exceed the sale's total amount. Remaining un-invoiced balance: {remaining:0.00} {sale.Currancy}.");
            }

            var invoice = request.Adapt<Invoice>();
            await _uow.Repository<Invoice>().CreateAsync(invoice); 
            await _uow.SaveChangesAsync();

            var created = await GetInvoice(i => i.Id == invoice.Id);

            return created is not null ?
                ServiceResult<InvoiceResponse>.Ok(created, "Invoice created successfully") :
                ServiceResult<InvoiceResponse>.Fail("Invoice was created but could not be reloaded");
        }

        public async Task<ServiceResult<InvoiceResponse>> UpdateInvoice(int id, UpdateInvoiceRequest request)
        {
            var invoice = await _uow.Repository<Invoice>().GetOneAsync(i => i.Id == id);
            if (invoice == null)
                return ServiceResult<InvoiceResponse>.NotFound("Invoice not found");

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Cancelled)
                return ServiceResult<InvoiceResponse>.Fail("Cannot edit a cancelled invoice");

            if (request.Amount.HasValue) 
            {
                if (invoice.InvoiceStatus != InvoiceStatusEnum.Draft)
                    return ServiceResult<InvoiceResponse>.Fail("Amount can only be changed while the invoice is still a Draft");

                var sale = await _uow.Repository<Sale>().GetOneAsync(
                    filter: s => s.Id == invoice.SaleId,
                    includes: new[] {nameof(Sale.Invoices)} 
                    );

                var otherInvoicesTotal = sale.Invoices
                .Where(i => i.Id != invoice.Id && i.EntityStatus != EntityStatusEnum.InActive && i.InvoiceStatus != InvoiceStatusEnum.Cancelled)
                .Sum(i => i.Amount);

                if (otherInvoicesTotal + request.Amount.Value > sale.Amount)
                {
                    var remaining = sale.Amount - otherInvoicesTotal;
                    return ServiceResult<InvoiceResponse>.Fail(
                        $"This invoice would exceed the sale's total amount. Remaining un-invoiced balance: {remaining:0.00} {sale.Currancy}.");
                }

                invoice.Amount = request.Amount.Value;

            }

            if(request.DueDate.HasValue)
                invoice.DueDate = request.DueDate.Value;

            _uow.Repository<Invoice>().Update( invoice );
            var affected = await _uow.SaveChangesAsync();
            if (affected == 0)
                return ServiceResult<InvoiceResponse>.Fail("No changes were made.");

            var updated = await GetInvoice(i=> i.Id == invoice.Id);

            return updated != null
            ? ServiceResult<InvoiceResponse>.Ok(updated, "Invoice updated successfully.")
            : ServiceResult<InvoiceResponse>.Fail("Invoice was updated but could not be reloaded.");
        }

        public async Task<ServiceResult<bool>> UpdateInvoiceStatus(int id, UpdateInvoiceStatusRequest request)
        {
            var invoice = await _uow.Repository<Invoice>().GetOneAsync(i => i.Id == id);
            if (invoice == null)
                return ServiceResult<bool>.NotFound("Invoice not found");

            if (!Enum.IsDefined(typeof(InvoiceStatusEnum), request.InvoiceStatus))
                return ServiceResult<bool>.Fail("Invalid status value.");

            if(invoice.InvoiceStatus == InvoiceStatusEnum.Cancelled)
                return ServiceResult<bool>.Fail("This Opportunity is already cancelled and cannot change state further");

            if (request.InvoiceStatus is InvoiceStatusEnum.Paid or InvoiceStatusEnum.PartiallyPaid or InvoiceStatusEnum.Overdue)
                return ServiceResult<bool>.Fail("This status is computed automatically from payments and cannot be set manually");

            var allowed = (invoice.InvoiceStatus, request.InvoiceStatus) switch
            {
                (InvoiceStatusEnum.Draft, InvoiceStatusEnum.Sent) => true,
                (InvoiceStatusEnum.Draft, InvoiceStatusEnum.Cancelled) => true,
                (InvoiceStatusEnum.Sent, InvoiceStatusEnum.Cancelled) => true,
                _ => false
            };

            if (!allowed)
                return ServiceResult<bool>.Fail($"Cannot move invoice from {invoice.InvoiceStatus} to {request.InvoiceStatus}.");

            invoice.InvoiceStatus = request.InvoiceStatus;
            _uow.Repository<Invoice>().Update(invoice);

            var affected = await _uow.SaveChangesAsync();
            return affected > 0
            ? ServiceResult<bool>.Ok(true, "Invoice status updated successfully")
            : ServiceResult<bool>.Fail("Failed to update invoice status");
        }

        public async Task<ServiceResult<bool>> DeleteInvoice(int id)
        {
            var invoice = await _uow.Repository<Invoice>().GetOneAsync(i => i.Id == id);

            if (invoice == null)
                return ServiceResult<bool>.NotFound("Invoice not found");

            if (invoice.InvoiceStatus is not (InvoiceStatusEnum.Draft or InvoiceStatusEnum.Cancelled))
                return ServiceResult<bool>.Fail("Only Draft or Cancelled invoices can be deleted. Cancel this invoice first if it must be removed.");

            invoice.EntityStatus = EntityStatusEnum.InActive;
            _uow.Repository<Invoice>().Update(invoice);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0 ?
                ServiceResult<bool>.Ok(true, "Invoice deleted successfully.")
                : ServiceResult<bool>.Fail("Failed to delete invoice");
        }

        public async Task<ServiceResult<InvoiceResponse>> RecordPayment(int invoiceId, RecordPaymentRequest request)
        {
            var invoice = await _uow.Repository<Invoice>().GetOneAsync(
            i => i.Id == invoiceId, includes: new[] { nameof(Invoice.Payments) });

            if (invoice is null)
                return ServiceResult<InvoiceResponse>.NotFound("Invoice not found.");

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Cancelled)
                return ServiceResult<InvoiceResponse>.Fail("Cannot record a payment against a cancelled invoice.");

            if (invoice.InvoiceStatus == InvoiceStatusEnum.Draft)
                return ServiceResult<InvoiceResponse>.Fail("Cannot record a payment against a Draft invoice - mark it Sent first.");

            if (request.PaymentMethod == PaymentMethodEnum.CreditCard)
                return ServiceResult<InvoiceResponse>.Fail("Card payments must go through the Stripe checkout endpoint, not recorded manually.");

            var committed = InvoiceStatusCalculator.GetCommittedAmount(invoice);
            var remaining = invoice.Amount - committed;

            if (request.Amount > remaining)
            {
                return ServiceResult<InvoiceResponse>.Fail(
                remaining <= 0
                    ? "This invoice is already fully covered by existing payments."
                    : $"Payment exceeds the remaining balance. Remaining: {remaining:0.00}");
            }

            var payment = new Payment
            {
                InvoiceId = invoiceId,
                Amount = request.Amount,
                PaymentMethod = request.PaymentMethod,
                PaymentDate = request.PaymentDate ?? DateTime.UtcNow,
                PaymentStatus = request.Status,
                ReferenceNumber = request.ReferenceNumber,
                Notes = request.Notes
            };

            await _uow.Repository<Payment>().CreateAsync(payment);

            invoice.Payments.Add(payment); // so ComputeStatus sees it immediately, no extra round trip
            invoice.InvoiceStatus = InvoiceStatusCalculator.ComputeStatus(invoice);

            _uow.Repository<Invoice>().Update(invoice);
            var affected = await _uow.SaveChangesAsync();

            if (affected == 0)
                return ServiceResult<InvoiceResponse>.Fail("Failed to record payment.");

            var updated = await GetInvoice(i => i.Id == invoiceId);
            return updated is not null
                ? ServiceResult<InvoiceResponse>.Ok(updated, "Payment recorded successfully.")
                : ServiceResult<InvoiceResponse>.Fail("Payment was recorded but the invoice could not be reloaded.");
        }
    }
}
