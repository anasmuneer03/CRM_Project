using CRM.BLL.Common;
using CRM.BLL.Common.CurrentUser;
using CRM.DAL.DTO.Request.Notes;
using CRM.DAL.DTO.Response.Notes;
using CRM.DAL.Models;
using CRM.DAL.Repository;
using CRM.DAL.Utils;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Notes
{
    public class NoteService : INoteService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUser;
        private static readonly string[] Includes = { nameof(Note.CreatedBy) };

        public NoteService(IUnitOfWork uow, ICurrentUserService currentUser)
        {
            _uow = uow;
            _currentUser = currentUser;
        }

        private bool CanModify(Note note) =>
        _currentUser.IsInRole(roleName: "Admin")
        || _currentUser.IsInRole(roleName: "Manager")
        || note.CreatedById == _currentUser.UserId;

        public async Task<List<NoteResponse>> GetAllNotes(Expression<Func<Note, bool>>? filter = null)
        {
            var notes = await _uow.Repository<Note>().GetAllAsync(filter, includes: Includes);
            return notes.Adapt<List<NoteResponse>>();
        }

        public async Task<NoteResponse?> GetNote(Expression<Func<Note, bool>> filter)
        {
            var note = await _uow.Repository<Note>().GetOneAsync(filter, includes: Includes);
            return note?.Adapt<NoteResponse>();
        }

        public async Task<ServiceResult<NoteResponse>> CreateNote(NoteRequest request)
        {
            var (isValid, error) = await ParentEntityValidator.ValidateExactlyOneParent(
            _uow, request.LeadId, request.CustomerId, request.OpportunityId);

            if (!isValid)
                return ServiceResult<NoteResponse>.Fail(error!);

            var note = request.Adapt<Note>();
            await _uow.Repository<Note>().CreateAsync(note);
            await _uow.SaveChangesAsync();

            var created = await GetNote(n => n.Id == note.Id);
            return created is not null
                ? ServiceResult<NoteResponse>.Ok(created, "Note created successfully.")
                : ServiceResult<NoteResponse>.Fail("Note was created but could not be reloaded.");
        }

        public async Task<ServiceResult<NoteResponse>> UpdateNote(int id, UpdateNoteRequest request)
        {
            var note = await _uow.Repository<Note>().GetOneAsync(filter: n => n.Id == id);
            if (note is null)
                return ServiceResult<NoteResponse>.NotFound("Note not found");

            if (!CanModify(note))
                return ServiceResult<NoteResponse>.Fail("You don't have permission to edit this note.");

            note.Content = request.Content;
            _uow.Repository<Note>().Update(note);
            var affected = await _uow.SaveChangesAsync();

            if (affected == 0)
                return ServiceResult<NoteResponse>.Fail("o changes were made");

            var updated = await GetNote(n => n.Id == note.Id);
            return updated != null
                ? ServiceResult<NoteResponse>.Ok(updated, "Note updated successfully.")
                : ServiceResult<NoteResponse>.Fail("Note was updated but could not be reloaded.");
        }

        public async Task<ServiceResult<bool>> DeleteNote(int id)
        {
            var note = await _uow.Repository<Note>().GetOneAsync(n => n.Id == id);

            if(note is null)
                return ServiceResult<bool>.NotFound("Note not found");

            if (!CanModify(note))
                return ServiceResult<bool>.Fail("You don't have permission to delete this note");
            

            note.EntityStatus = EntityStatusEnum.InActive;
            _uow.Repository<Note>().Update(note);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0
                ? ServiceResult<bool>.Ok(true, "Note deleted successfully.")
                : ServiceResult<bool>.Fail("Failed to delete note");

        }
    }
}
