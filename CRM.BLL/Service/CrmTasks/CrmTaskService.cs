using CRM.BLL.Common;
using CRM.BLL.Common.CurrentUser;
using CRM.DAL.DTO.Request.CrmTasks;
using CRM.DAL.DTO.Response.CrmTasks;
using CRM.DAL.Models;
using CRM.DAL.Repository;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.CrmTasks
{
    public class CrmTaskService : ICrmTaskService
    {
        private readonly IUnitOfWork _uow;
        private readonly ICurrentUserService _currentUser;
        private static readonly string[] Includes = { nameof(CrmTask.CreatedBy), nameof(CrmTask.AssignedTo) };

        public CrmTaskService(IUnitOfWork uow, ICurrentUserService currentUser)
        {
            _uow = uow;
            _currentUser = currentUser;
        }

        private bool CanModify(CrmTask task) =>
            _currentUser.IsInRole(roleName: "Admin") ||
            _currentUser.IsInRole(roleName: "Manager") ||
            task.CreatedById == _currentUser.UserId||
            (task.AssignedToId != null && task.AssignedToId == _currentUser.UserId);

        private static bool ComputeIsOverdue(CrmTask task) =>
           task.DueDate < DateTime.UtcNow
           && task.Status is not (TaskStatusEnum.Completed or TaskStatusEnum.Cancelled);

        public async Task<List<CrmTaskResponse>> GetAllTasks(Expression<Func<CrmTask, bool>> filter)
        {
            var tasks = await _uow.Repository<CrmTask>().GetAllAsync(filter, includes: Includes);
            
            var responses = tasks.Adapt<List<CrmTaskResponse>>();
            for (var i = 0; i < tasks.Count; i++)
                responses[i].IsOverdue = ComputeIsOverdue(tasks[i]);

            return responses;
        }

        public async Task<CrmTaskResponse?> GetTask(Expression<Func<CrmTask, bool>> filter)
        {
            var task = await _uow.Repository<CrmTask>().GetOneAsync(filter, includes: Includes);
            if(task is  null)
                return null;

            var response = task.Adapt<CrmTaskResponse>();
            response.IsOverdue = ComputeIsOverdue(task);
            return response;
        }

        public async Task<List<CrmTaskResponse>> GetMyTasks()
        {
            return await GetAllTasks(t => t.AssignedToId == _currentUser.UserId);
        }

        public async Task<ServiceResult<CrmTaskResponse>> CreateTask(CrmTaskRequest request)
        {
            var (isValid, error) = await ParentEntityValidator.ValidateExactlyOneParent(
            _uow, request.LeadId, request.CustomerId, request.OpportunityId);

            if (!isValid)
                return ServiceResult<CrmTaskResponse>.Fail(error!);

            if (request.Priority.HasValue && !Enum.IsDefined(typeof(TaskPriorityEnum), request.Priority.Value))
                return ServiceResult<CrmTaskResponse>.Fail("Invalid priority value.");

            var task = request.Adapt<CrmTask>();
            task.Status = TaskStatusEnum.Pending;
            task.Priority = request.Priority ?? TaskPriorityEnum.Medium;

            if (request.AssignedToId == null)
                task.AssignedToId = _currentUser.UserId;

            await _uow.Repository<CrmTask>().CreateAsync(task);
            await _uow.SaveChangesAsync();

            var created = await GetTask(t=> t.Id == task.Id);
            return created is not null
                ? ServiceResult<CrmTaskResponse>.Ok(created, "Task created successfully.")
                : ServiceResult<CrmTaskResponse>.Fail("Task was created but could not be reloaded.");
        }

        public async Task<ServiceResult<CrmTaskResponse>> UpdateTask(int id, UpdateCrmTaskRequest request)
        {
            var task = await _uow.Repository<CrmTask>().GetOneAsync(filter: t=> t.Id == id);
            if (task == null)
                return ServiceResult<CrmTaskResponse>.NotFound("Task not found");

            if (!CanModify(task))
                return ServiceResult<CrmTaskResponse>.Fail("You don't have permission to edit this task.");

            if (request.Priority.HasValue && !Enum.IsDefined(typeof(TaskPriorityEnum), request.Priority.Value))
            {
                return ServiceResult<CrmTaskResponse>.Fail("Invalid priority value.");
            }
            
            if(request.Priority.HasValue && Enum.IsDefined(typeof(TaskPriorityEnum), request.Priority.Value))
                task.Priority = request.Priority.Value;
     
            if (request.Title != null)
                task.Title = request.Title;

            if(request.Description != null)
                task.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description;

            if (request.DueDate.HasValue)
                task.DueDate = request.DueDate.Value;


            _uow.Repository<CrmTask>().Update(task);
            var affected = await _uow.SaveChangesAsync();

            if (affected == 0)
                return ServiceResult<CrmTaskResponse>.Fail("No changes were made");

            var updatedTask = await GetTask(t => t.Id == task.Id);

            return updatedTask != null ?
                ServiceResult<CrmTaskResponse>.Ok(updatedTask, "Task updated successfully") :
                ServiceResult<CrmTaskResponse>.Fail("Task was updated but could not be reloaded");

        }

        public async Task<ServiceResult<bool>> UpdateTaskStatus(int id, UpdateCrmTaskStatusRequest request)
        {
            var task = await _uow.Repository<CrmTask>().GetOneAsync(t => t.Id == id);
            if (task == null)
                return ServiceResult<bool>.NotFound("Task not found");

            if (!CanModify(task))
                return ServiceResult<bool>.Fail("You don't have permission to update this task status");

            if (!Enum.IsDefined(typeof(TaskStatusEnum), request.Status))
                return ServiceResult<bool>.Fail("Invalid status value.");

            if(task.Status == TaskStatusEnum.Completed || task.Status == TaskStatusEnum.Cancelled)
                return ServiceResult<bool>.Fail($"Can't update status already {task.Status}.");

            task.Status = request.Status;
            task.CompletedAt = request.Status == TaskStatusEnum.Completed ? DateTime.UtcNow : null;

            _uow.Repository<CrmTask>().Update(task);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0
                ? ServiceResult<bool>.Ok(true, "Task status updated.")
                : ServiceResult<bool>.Fail("Failed to update task status.");
        }

        public async Task<ServiceResult<bool>> ReassignTask(int id, ReassignTaskRequest request)
        {
            var task = await _uow.Repository<CrmTask>().GetOneAsync(t => t.Id == id);
            if (task == null)
                return ServiceResult<bool>.NotFound("Task not found");

            if (!(_currentUser.IsInRole(roleName: "Admin") || _currentUser.IsInRole(roleName: "Manager") || task.CreatedById == _currentUser.UserId))
                return ServiceResult<bool>.Fail("You don't have permission to reassign this task.");

            task.AssignedToId = string.IsNullOrWhiteSpace(request.AssignedToId) ? null : request.AssignedToId;

            _uow.Repository<CrmTask>().Update(task);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0
                ? ServiceResult<bool>.Ok(true, "Task reassigned.")
                : ServiceResult<bool>.Fail("Failed to reassign task.");
        }


        public async Task<ServiceResult<bool>> DeleteTask(int id)
        {
            var task = await _uow.Repository<CrmTask>().GetOneAsync(t=> t.Id == id);
            if (task == null)
                return ServiceResult<bool>.NotFound("Task not found");

            if (!CanModify(task))
                return ServiceResult<bool>.Fail("You don't have permission to delete this task");

            if (task.Status == TaskStatusEnum.Cancelled)
                return ServiceResult<bool>.Fail("Task status already cancalled");

            task.EntityStatus = EntityStatusEnum.InActive;
            _uow.Repository<CrmTask>().Update(task);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0
                ? ServiceResult<bool>.Ok(true, "Task deleted successfully.")
                : ServiceResult<bool>.Fail("Failed to delete task");
        }
    }
}
