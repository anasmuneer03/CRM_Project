using CRM.BLL.Service.Invoices;
using CRM.DAL.DTO.Request.Invoices;
using CRM.DAL.DTO.Request.Opportunities;
using CRM.PL.Common;
using CRM.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CRM.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoicesController : BaseApiController
    {
        private readonly IInvoiceService _invoiceService;
        private readonly IStringLocalizer _stringLocalizer;
        public InvoicesController(IInvoiceService invoiceService,
            IStringLocalizer<SharedResources> stringLocalizer)
        {
            _invoiceService = invoiceService;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var invoices = await _invoiceService.GetAllInvoices();
            return SuccessResponse(invoices, _stringLocalizer["Success"].Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var invoice = await _invoiceService.GetInvoice(i => i.Id == id);
            if (invoice == null)
                return NotFoundResponse("Invoice not found");
            return SuccessResponse(invoice, _stringLocalizer["Success"].Value);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] InvoiceRequest request)
        {
            var created = await _invoiceService.CreateInvoice(request);
            if (!created.Success)
                return created.IsNotFound ? NotFoundResponse(created.Message) : BadRequestResponse(created.Message);
            return CreatedResponse(nameof(GetOne), new { id = created.Data?.Id }, created.Data, created.Message);

        }

        [HttpPatch("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateInvoiceRequest request)
        {
            var updated = await _invoiceService.UpdateInvoice(id, request);
            if (!updated.Success)
            {
                return updated.IsNotFound ? NotFoundResponse(updated.Message) : BadRequestResponse(updated.Message);
            }
            return SuccessResponse(updated.Data, updated.Message);
        }

        [HttpPatch("{id:int}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateInvoiceStatusRequest request)
        {
            var statusUpdated = await _invoiceService.UpdateInvoiceStatus(id, request);
            if (!statusUpdated.Success)
            {
                return statusUpdated.IsNotFound ? NotFoundResponse(statusUpdated.Message) :
                   BadRequestResponse(statusUpdated.Message);
            }
            return SuccessResponse(statusUpdated.Message);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _invoiceService.DeleteInvoice(id);
            if (!deleted.Success)
                return deleted.IsNotFound ? NotFoundResponse(deleted.Message) : BadRequestResponse(deleted.Message);
            return SuccessResponse(deleted.Message);
        }

        [HttpPost("{id:int}/payments")]
        [Authorize]
        public async Task<IActionResult> RecordPayment(int id, RecordPaymentRequest request)
        {
            var recorded = await _invoiceService.RecordPayment(id, request);
            if(!recorded.Success)
                return recorded.IsNotFound ? NotFoundResponse(recorded.Message) : BadRequestResponse(recorded.Message);
            return SuccessResponse(recorded.Data, recorded.Message);
        }
    }
}
