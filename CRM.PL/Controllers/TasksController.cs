using CRM.BLL.Service.CrmTasks;
using CRM.DAL.DTO.Request.CrmTasks;
using CRM.DAL.DTO.Request.Notes;
using CRM.DAL.Models;
using CRM.PL.Common;
using CRM.PL.Resources;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace CRM.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController :BaseApiController
    {
        private readonly ICrmTaskService _taskService;
        private readonly IStringLocalizer _stringLocalizer;
        public TasksController(ICrmTaskService taskService,
            IStringLocalizer<SharedResources> stringLocalizer)
        {
            _taskService = taskService;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? leadId,
            [FromQuery] int? customerId, [FromQuery] int? opportunityId)
        {
            var tasks = await _taskService.GetAllTasks(
                t => (!leadId.HasValue || t.LeadId == leadId)
                 && (!customerId.HasValue || t.CustomerId == customerId)
                 && (!opportunityId.HasValue || t.OpportunityId == opportunityId));

            return SuccessResponse(tasks, _stringLocalizer["Success"].Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var task = await _taskService.GetTask(t => t.Id == id);
            if (task == null)
                return NotFoundResponse(_stringLocalizer["TaskNotFound"].Value);

            return SuccessResponse(task, _stringLocalizer["Success"].Value);
        }

        [HttpGet("mine")]
        [Authorize]
        public async Task<IActionResult> GetMine()
        {
            var tasks = await _taskService.GetMyTasks();
            return SuccessResponse(tasks, _stringLocalizer["Success"].Value);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CrmTaskRequest request)
        {
            var created = await _taskService.CreateTask(request);
            if (!created.Success)
                return BadRequestResponse(created.Message);
            return CreatedResponse(nameof(GetOne), new { id = created.Data!.Id }, created.Data, created.Message);
        }

        [HttpPatch("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateCrmTaskRequest request)
        {
            var updated = await _taskService.UpdateTask(id, request);

            if (!updated.Success)
            {
                return updated.IsNotFound
                    ? NotFoundResponse(updated.Message)
                    : BadRequestResponse(updated.Message);
            }
            return SuccessResponse(updated.Data, updated.Message);
        }

        [HttpPatch("{id:int}/status")]
        [Authorize]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateCrmTaskStatusRequest request)
        {
            var statusUpdated = await _taskService.UpdateTaskStatus(id, request);
            if (!statusUpdated.Success)
            {
                return statusUpdated.IsNotFound
                    ? NotFoundResponse(statusUpdated.Message)
                    : BadRequestResponse(statusUpdated.Message);
            }
            return SuccessResponse(statusUpdated.Message);
        }

        [HttpPatch("{id:int}/reassign")]
        [Authorize]
        public async Task<IActionResult> ReAssignOne(int id, ReassignTaskRequest request)
        {
            var reAssigned = await _taskService.ReassignTask(id, request);
            if (!reAssigned.Success)
            {
                return reAssigned.IsNotFound
                    ? NotFoundResponse(reAssigned.Message)
                    : BadRequestResponse(reAssigned.Message);
            }
            return SuccessResponse(reAssigned.Message);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _taskService.DeleteTask(id);
            if (!deleted.Success)
            {
                return deleted.IsNotFound
                    ? NotFoundResponse(deleted.Message)
                    : BadRequestResponse(deleted.Message);
            }
            return SuccessResponse(deleted.Message);
        }


    }
}
