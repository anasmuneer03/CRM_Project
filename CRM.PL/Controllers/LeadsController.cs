using CRM.BLL.Service.Leads;
using CRM.DAL.DTO.Request.Leads;
using CRM.DAL.Models;
using CRM.PL.Common;
using CRM.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRM.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LeadsController : BaseApiController
    {
        private readonly ILeadService _leadService;
        private readonly IStringLocalizer _stringLocalizer;

        public LeadsController(ILeadService leadService, IStringLocalizer<SharedResources> stringLocalizer)
        {
            _leadService = leadService;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var leads = await _leadService.GetAllLeads();
            return SuccessResponse(leads, _stringLocalizer["Success"].Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var lead = await _leadService.GetLead(l => l.Id == id);
            if (lead is null)
                return NotFoundResponse(_stringLocalizer["LeadNotFound"].Value);
            return SuccessResponse(lead, _stringLocalizer["Success"].Value);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] LeadRequest request)
        {
            var created = await _leadService.CreateLead(request);
            return CreatedResponse(nameof(GetOne), new { id = created.Id}, created, _stringLocalizer["Created"].Value);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _leadService.DeleteLead(id);
            if (!deleted.Success)
                return deleted.IsNotFound ? NotFoundResponse(deleted.Message) : BadRequestResponse(deleted.Message);
            return SuccessResponse(deleted.Message);
        }

        [HttpPatch("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody]UpdateLeadRequest request)
        {
            var updated = await _leadService.UpdateLead(id, request);
            if (!updated.Success)
            {
                return updated.IsNotFound ? NotFoundResponse(updated.Message)
                    : BadRequestResponse(updated.Message);
            }
            return SuccessResponse(updated.Data ,updated.Message);
        }

        [HttpPatch("{id:int}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int id, UpdateStatusRequest request)
        {
            var updated = await _leadService.UpdateLeadStatus(id, request);
            if (!updated.Success) 
            {
                return updated.IsNotFound ? NotFoundResponse(updated.Message)
                : BadRequestResponse(updated.Message);
            }
            return SuccessResponse(updated.Message);
        }

        [HttpPost("{id:int}/convert")]
        [Authorize]
        public async Task<IActionResult> ConvertToCustomer(int id, ConvertToCustomerRequest request)
        {
            var converted = await _leadService.ConvertLeadToCustomer(id, request);
            if (!converted.Success) 
            {
                return converted.IsNotFound ? NotFoundResponse(converted.Message)
                    : BadRequestResponse(converted.Message);
            }
            return SuccessResponse(converted.Data, converted.Message);
        }
    }
}
