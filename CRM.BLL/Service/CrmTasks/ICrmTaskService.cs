using CRM.BLL.Common;
using CRM.DAL.DTO.Request.CrmTasks;
using CRM.DAL.DTO.Response.CrmTasks;
using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.CrmTasks
{
    public interface ICrmTaskService
    {
        Task<List<CrmTaskResponse>> GetAllTasks(Expression<Func<CrmTask,bool>> filter);
        Task<CrmTaskResponse?> GetTask(Expression<Func<CrmTask, bool>> filter);
        Task<List<CrmTaskResponse>> GetMyTasks();
        Task<ServiceResult<CrmTaskResponse>> CreateTask(CrmTaskRequest request);
        Task<ServiceResult<CrmTaskResponse>> UpdateTask(int id, UpdateCrmTaskRequest request);
        Task<ServiceResult<bool>> UpdateTaskStatus(int id, UpdateCrmTaskStatusRequest request);
        Task<ServiceResult<bool>> ReassignTask(int id, ReassignTaskRequest request);
        Task<ServiceResult<bool>> DeleteTask(int id);
    }
}
