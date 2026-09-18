using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Leads;
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
    public interface IOpportunityService
    {
        Task<List<OpportunityResponse>> GetAllLOpportunities();
        Task<OpportunityResponse?> GetOpportunity(Expression<Func<Opportunity, bool>> filter);
        Task<OpportunityResponse> CreateOpportunity(OpportunityRequest request);
        Task<ServiceResult<OpportunityResponse>> UpdateOpportunity(int id, UpdateOpportunityRequest request);
        Task<ServiceResult<bool>> UpdateOpportunityStage(int id, UpdateOpportunityStageRequest request);
        Task<ServiceResult<CloseOpportunityAsWonResponse>> CloseOpportunityAsWon(int id);
        Task<ServiceResult<bool>> DeleteOpportunity(int id);
    }
}
