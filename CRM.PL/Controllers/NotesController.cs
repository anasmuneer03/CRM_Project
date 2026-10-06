using CRM.BLL.Service.Notes;
using CRM.DAL.DTO.Request.Notes;
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
    public class NotesController :BaseApiController
    {
        private readonly INoteService _noteService;
        private readonly IStringLocalizer _stringLocalizer;

        public NotesController(INoteService noteService, 
            IStringLocalizer<SharedResources> stringLocalizer)
        {
            _noteService = noteService;
            _stringLocalizer = stringLocalizer;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int? leadId,
            [FromQuery] int? customerId, [FromQuery] int? opportunityId)
        {
            var notes = await _noteService.GetAllNotes(
            n => (!leadId.HasValue || n.LeadId == leadId)
                 && (!customerId.HasValue || n.CustomerId == customerId)
                 && (!opportunityId.HasValue || n.OpportunityId == opportunityId));

            return SuccessResponse(notes, _stringLocalizer["Success"].Value);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOne(int id)
        {
            var note = await _noteService.GetNote(n => n.Id == id);
            if(note  == null)
                return NotFoundResponse(_stringLocalizer["NoteNotFound"].Value);

            return SuccessResponse(note, _stringLocalizer["Success"].Value);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] NoteRequest request)
        {
            var created = await _noteService.CreateNote(request);
            if (!created.Success)
                return BadRequestResponse(created.Message);
            return CreatedResponse(nameof(GetOne), new { id = created.Data!.Id }, created.Data, created.Message);
        }

        [HttpPatch("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Update(int id, [FromBody]UpdateNoteRequest request)
        {
            var updated = await _noteService.UpdateNote(id, request);

            if (!updated.Success) 
            {
                return updated.IsNotFound
                    ? NotFoundResponse(updated.Message)
                    : BadRequestResponse(updated.Message);
            }
            return SuccessResponse(updated.Data, updated.Message);
        }

        [HttpDelete("{id:int}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _noteService.DeleteNote(id);

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
