using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Notes;
using CRM.DAL.DTO.Response.Notes;
using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Notes
{
    public interface INoteService
    {
        Task<List<NoteResponse>> GetAllNotes(Expression<Func<Note, bool>>? filter = null);
        Task<NoteResponse?> GetNote(Expression<Func<Note, bool>> filter);
        Task<ServiceResult<NoteResponse>> CreateNote(NoteRequest request);
        Task<ServiceResult<NoteResponse>> UpdateNote(int id, UpdateNoteRequest request);
        Task<ServiceResult<bool>> DeleteNote(int id);
    
    }
}
