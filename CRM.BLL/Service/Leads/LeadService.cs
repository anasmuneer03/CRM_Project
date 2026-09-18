using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Leads;
using CRM.DAL.DTO.Response.Leads;
using CRM.DAL.Models;
using CRM.DAL.Repository;
using Mapster;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Leads
{
    public class LeadService : ILeadService
    {
        private readonly IUnitOfWork _uow;
        public LeadService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<List<LeadResponse>> GetAllLeads()
        {
            var leads = await _uow.Repository<Lead>().GetAllAsync(
                includes: new string[] {nameof(Lead.AssignedAgent)}); 
            return leads.Adapt<List<LeadResponse>>();
        }

        public async Task<LeadResponse?> GetLead(Expression<Func<Lead, bool>> filter)
        {
            var lead = await _uow.Repository<Lead>().GetOneAsync(filter);
            return lead?.Adapt<LeadResponse>();
        }
        public async Task<LeadResponse> CreateLead(LeadRequest request)
        { 
            var lead = request.Adapt<Lead>();
            await _uow.Repository<Lead>().CreateAsync(lead);
            await _uow.SaveChangesAsync();
            return lead.Adapt<LeadResponse>();
        }

        public async Task<ServiceResult<bool>> DeleteLead(int id)
        {
            var lead = await _uow.Repository<Lead>().GetOneAsync(filter: l => l.Id == id);
            if (lead is null)
               return ServiceResult<bool>.NotFound("Lead not found.");

            _uow.Repository<Lead>().Delete(lead);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0
            ? ServiceResult<bool>.Ok(true, "Lead deleted successfully.")
            : ServiceResult<bool>.Fail("Failed to delete lead.");
        }

        public async Task<ServiceResult<LeadResponse>> UpdateLead(int id, UpdateLeadRequest request)
        {
            var lead = await _uow.Repository<Lead>().GetOneAsync(filter: l => l.Id == id);

            if (lead is null) return ServiceResult<LeadResponse>.NotFound("Lead not found");

            if (lead.LeadStatus == LeadStatusEnum.Converted)
                return ServiceResult<LeadResponse>.Fail(
                    "Cannot edit a lead that has already been converted to a customer"
                    );

            if (request.FullName != null)
                lead.FullName = request.FullName;

            if (request.Email != null)
                lead.Email = request.Email;

            if (request.Phone != null)
                lead.Phone = string.IsNullOrWhiteSpace(request.Phone) ? null : request.Phone;

            if (request.CompanyName != null)
                lead.CompanyName = string.IsNullOrWhiteSpace(request.CompanyName) ? null : request.CompanyName;

            if (request.LeadSource.HasValue)
                lead.LeadSource = request.LeadSource.Value;

            if (request.AssignedAgentId != null)
                lead.AssignedAgentId = string.IsNullOrWhiteSpace(request.AssignedAgentId) ? null : request.AssignedAgentId;

            _uow.Repository<Lead>().Update(lead);
            var affected = await _uow.SaveChangesAsync();

            if (affected == 0)
                return ServiceResult<LeadResponse>.Fail("No changes were made");

            var updated = await GetLead(l => l.Id == id);

            return updated is not null ?
                ServiceResult<LeadResponse>.Ok(updated, "Lead updated successfully")
                : ServiceResult<LeadResponse>.Fail("Lead was updated but could not be reloaded");
        }

        public async Task<ServiceResult<bool>> UpdateLeadStatus(int id, UpdateStatusRequest request)
        {
            var lead = await _uow.Repository<Lead>().GetOneAsync(
                filter: l => l.Id == id);
            if (lead is null)
                return ServiceResult<bool>.NotFound("Lead is not found");

            if (request.LeadStatus == LeadStatusEnum.Converted)
                return ServiceResult<bool>.Fail("Use the dedicated convert endpoint to move a lead to Converted status");

            if (request.LeadStatus == LeadStatusEnum.Lost && string.IsNullOrWhiteSpace(request.LostReason))
                return ServiceResult<bool>.Fail("A reason is required when marking a lead as Lost");

            lead.LeadStatus = request.LeadStatus;
            lead.LostReason = request.LeadStatus == LeadStatusEnum.Lost ? request.LostReason : null;

            _uow.Repository<Lead>().Update(lead);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0
                ? ServiceResult<bool>.Ok(true, "Lead status updated.")
                : ServiceResult<bool>.Fail("Failed to update lead status.");
        }

        public async Task<ServiceResult<ConvertToCustomerResponse>> ConvertLeadToCustomer(int id, ConvertToCustomerRequest request)
        {
            var lead = await _uow.Repository<Lead>().GetOneAsync(l => l.Id == id);
            if (lead is null) return ServiceResult<ConvertToCustomerResponse>.NotFound("Lead not found");
            if (lead.LeadStatus == LeadStatusEnum.Converted)
                return ServiceResult<ConvertToCustomerResponse>.Fail("This lead has already been converted");

            var phone = lead.Phone ?? request.Phone;
            if (string.IsNullOrWhiteSpace(phone))
            {
                return ServiceResult<ConvertToCustomerResponse>.Fail(
                    "A phone number is required to convert this lead - the lead has none on file, please provide one"
                    );
            }

            using var transaction = await _uow.BeginTransactionAsync();
            try
            {

                var customer = new Customer
                {
                    FullName = lead.FullName,
                    Email = lead.Email,
                    Phone = phone,
                    Address = request.Address,
                    CompanyName = lead.CompanyName,
                    AssignedAgentId = lead.AssignedAgentId,
                    ConvertedFromLeadId = lead.Id,
                };

                await _uow.Repository<Customer>().CreateAsync(customer);
                await _uow.SaveChangesAsync();

                lead.LeadStatus = LeadStatusEnum.Converted;
                lead.ConvertedToCustomerId = customer.Id;
                lead.ConvertedAt = DateTime.UtcNow;

                _uow.Repository<Lead>().Update(lead);
                await _uow.SaveChangesAsync();

                await transaction.CommitAsync();

                return ServiceResult<ConvertToCustomerResponse>.Ok(
                    new ConvertToCustomerResponse
                    {
                        CustomerId = customer.Id,
                        LeadId = lead.Id,
                        ConvertedAt = lead.ConvertedAt.Value
                    }, "Lead successfully converted to customer.");
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

    }
}
