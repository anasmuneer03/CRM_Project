using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Leads;
using CRM.DAL.DTO.Response.Leads;
using CRM.DAL.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Leads
{
    public interface ILeadService
    {
        Task<List<LeadResponse>> GetAllLeads();
        Task<LeadResponse?> GetLead(Expression<Func<Lead, bool>> filter);
        Task<LeadResponse> CreateLead(LeadRequest request);
        Task<ServiceResult<bool>> DeleteLead(int id);
        Task<ServiceResult<LeadResponse>> UpdateLead(int id, UpdateLeadRequest request);
        Task<ServiceResult<bool>> UpdateLeadStatus(int id, UpdateStatusRequest request);
        Task<ServiceResult<ConvertToCustomerResponse>> ConvertLeadToCustomer(int id, ConvertToCustomerRequest request);
    }
}
