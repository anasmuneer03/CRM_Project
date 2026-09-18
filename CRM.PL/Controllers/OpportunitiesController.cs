using Azure.Core;
using CRM.BLL.Service.Customers;
using CRM.BLL.Service.Opportunities;
using CRM.DAL.DTO.Request.Customers;
using CRM.DAL.DTO.Request.Opportunities;
using CRM.DAL.Models;
using CRM.PL.Common;
using CRM.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace CRM.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpportunitiesController : BaseApiController
    {
        private readonly IOpportunityService _opportunityService;
        private readonly IStringLocalizer _stringLocalizer;
        public OpportunitiesController(IOpportunityService opportunityService,
            IStringLocalizer<SharedResources> stringLocalizer)
        {
            _opportunityService = opportunityService;
            _stringLocalizer = stringLocalizer;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _opportunityService.GetAllLOpportunities();
            return SuccessResponse(result, _stringLocalizer["Success"].Value);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var opportunity = await _opportunityService.GetOpportunity(o => o.Id == id);
            if (opportunity is null)
                return NotFoundResponse(_stringLocalizer["OpportunityNotFound"].Value);
            return SuccessResponse(opportunity, _stringLocalizer["Success"].Value);
        }

        [HttpPost()]
        [Authorize]
        public async Task<IActionResult> Create(OpportunityRequest request)
        {
            var created = await _opportunityService.CreateOpportunity(request);
            return CreatedResponse(nameof(GetOne), new { id = created.Id }, created, _stringLocalizer["Created"].Value);
        }

        [HttpPatch("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, UpdateOpportunityRequest request)
        {
            var updated = await _opportunityService.UpdateOpportunity(id, request);
            if (!updated.Success)
            {
                return updated.IsNotFound ? NotFoundResponse(updated.Message) : BadRequestResponse(updated.Message);
            }
            return SuccessResponse(updated.Data, updated.Message);
        }

        [HttpPatch("{id:int}/stage")]
        [Authorize]
        public async Task<IActionResult> UpdateStage(int id, [FromBody] UpdateOpportunityStageRequest request)
        {
            var stageUpdated = await _opportunityService.UpdateOpportunityStage(id, request);
            if (!stageUpdated.Success)
            {
                return stageUpdated.IsNotFound ? NotFoundResponse(stageUpdated.Message) :
                   BadRequestResponse(stageUpdated.Message);
            }
            return SuccessResponse(stageUpdated.Message);
        }

        [HttpPatch("{id:int}/close-won")]
        [Authorize]
        public async Task<IActionResult> CloseOpportunityAsWon(int id)
        {
            var result = await _opportunityService.CloseOpportunityAsWon(id);
            if (!result.Success)
            {
                return result.IsNotFound ? NotFoundResponse(result.Message) :
                   BadRequestResponse(result.Message);
            }
            return SuccessResponse(result.Data, result.Message);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _opportunityService.DeleteOpportunity(id);
            if (!deleted.Success)
            {
                return deleted.IsNotFound ? NotFoundResponse(deleted.Message) :
                   BadRequestResponse(deleted.Message);
            }
            return SuccessResponse(deleted.Message);
        }
    }
}
