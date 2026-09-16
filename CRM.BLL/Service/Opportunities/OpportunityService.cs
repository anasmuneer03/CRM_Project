using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Opportunities;
using CRM.DAL.DTO.Response.Leads;
using CRM.DAL.DTO.Response.Opportunities;
using CRM.DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Service.Opportunities
{
    public class OpportunityService : IOpportunityService
    {
        public Task<List<OpportunityResponse>> GetAllLOpportunities()
        {
            throw new NotImplementedException();
        }

        public Task<OpportunityResponse?> GetOpportunity(Expression<Func<Lead, bool>> filter)
        {
            throw new NotImplementedException();
        }
        public Task<OpportunityResponse> CreateOpportunity(OpportunityRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<LeadResponse>> UpdateOpportunity(int id, UpdateOpportunityRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<ServiceResult<bool>> UpdateOpportunityStatus(int id, UpdateOpportunityStatusRequest request)
        {
            throw new NotImplementedException();
        }
        public Task<ServiceResult<bool>> DeleteOpportunity(int id)
        {
            throw new NotImplementedException();
        }
        
    }
}
