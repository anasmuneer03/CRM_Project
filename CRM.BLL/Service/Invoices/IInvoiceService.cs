using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Invoices;
using CRM.DAL.DTO.Request.Opportunities;
using CRM.DAL.DTO.Response.Invoices;
using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Invoices
{
    public interface IInvoiceService
    {
        Task<List<InvoiceResponse>> GetAllInvoices();
        Task<InvoiceResponse?> GetInvoice(Expression<Func<Invoice, bool>> filter);
        Task<ServiceResult<InvoiceResponse>> CreateInvoice(InvoiceRequest request);
        Task<ServiceResult<InvoiceResponse>> UpdateInvoice(int id, UpdateInvoiceRequest request);
        Task<ServiceResult<bool>> UpdateInvoiceStatus(int id, UpdateInvoiceStatusRequest request);
        Task<ServiceResult<bool>> DeleteInvoice(int id);
        Task<ServiceResult<InvoiceResponse>> RecordPayment(int invoiceId, RecordPaymentRequest request);

    }
}
