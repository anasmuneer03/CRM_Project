using CRM.BLL.Common;
using CRM.DAL.DTO.Request.Opportunities;
using CRM.DAL.DTO.Response.Customers;
using CRM.DAL.DTO.Response.Leads;
using CRM.DAL.DTO.Response.Opportunities;
using CRM.DAL.Models;
using CRM.DAL.Repository;
using Mapster;
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
        private readonly IUnitOfWork _uow;

        private static bool isClosed(OpportunityStageEnum opportunityStage)
            => opportunityStage is 
            OpportunityStageEnum.ClosedWon or OpportunityStageEnum.ClosedLost;
        public OpportunityService(IUnitOfWork uow)
        {
            _uow = uow;
        }
        public async Task<List<OpportunityResponse>> GetAllLOpportunities()
        {
            var opportunities = await _uow.Repository<Opportunity>().GetAllAsync(
                includes: new string []{ nameof(Opportunity.Customer),
                nameof(Opportunity.AssignedAgent)} );
            return opportunities.Adapt<List<OpportunityResponse>>();
        }

        public async Task<OpportunityResponse?> GetOpportunity(Expression<Func<Opportunity, bool>> filter)
        {
            var opportunity = await _uow.Repository<Opportunity>().GetOneAsync(filter,
                includes: new string[]{ nameof(Opportunity.Customer),
                nameof(Opportunity.AssignedAgent)});
            return opportunity?.Adapt<OpportunityResponse>();
        }
        public async Task<OpportunityResponse> CreateOpportunity(OpportunityRequest request)
        {
            var opportunity = request.Adapt<Opportunity>();
            await _uow.Repository<Opportunity>().CreateAsync(opportunity);
            await _uow.SaveChangesAsync();
            return opportunity.Adapt<OpportunityResponse>();
        }

        public async Task<ServiceResult<OpportunityResponse>> UpdateOpportunity(int id, UpdateOpportunityRequest request)
        {
            var opportunity = await _uow.Repository<Opportunity>().GetOneAsync(
                filter: o => o.Id == id);
            if (opportunity == null)
                return ServiceResult<OpportunityResponse>.NotFound("Opportunity not found");

            if (isClosed(opportunity.Stage))
                return ServiceResult<OpportunityResponse>.Fail("Cannot edit an opportunity that has already been closed");

            if(request.Title != null)
                opportunity.Title = request.Title;

            if(request.Amount.HasValue)
                opportunity.Amount = request.Amount.Value;

            if(request.Probability.HasValue)
                opportunity.Probability = request.Probability.Value;

            if (request.Currancy.HasValue)
                opportunity.Currancy = request.Currancy.Value;

            if (request.ExpectedClosedDate.HasValue)
                opportunity.ExpectedClosedDate = request.ExpectedClosedDate.Value;

            if (request.Description != null)
                opportunity.Description = string.IsNullOrWhiteSpace(request.Description) ? null : request.Description;

            if (request.AssignedAgentId != null)
                opportunity.AssignedAgentId = string.IsNullOrWhiteSpace(request.AssignedAgentId) ? null : request.AssignedAgentId;

            _uow.Repository<Opportunity>().Update(opportunity);
            var affected = await _uow.SaveChangesAsync();

            if (affected == 0)
                return ServiceResult<OpportunityResponse>.Fail("No changes were made");

            var updated = await GetOpportunity(o => o.Id == id);

            return updated is not null ?
                ServiceResult<OpportunityResponse>.Ok(updated, "Opportunity updated successfully")
                : ServiceResult<OpportunityResponse>.Fail("Opportunity was updated but could not be reloaded");
        }

        public async Task<ServiceResult<bool>> UpdateOpportunityStage(int id, UpdateOpportunityStageRequest request)
        {
            var opportunity = await _uow.Repository<Opportunity>().GetOneAsync(o => o.Id == id);
            if (opportunity == null)
                return ServiceResult<bool>.NotFound("Opportunity not found");

            if (!Enum.IsDefined(typeof(OpportunityStageEnum), request.Stage))
                return ServiceResult<bool>.Fail("Invalid stage value.");

            if (isClosed(opportunity.Stage))
                return ServiceResult<bool>.Fail($"This opportunity is already {opportunity.Stage} and cannot change stage further");

            if (request.Stage == OpportunityStageEnum.ClosedWon)
                return ServiceResult<bool>.Fail("Use the dedicated close-won endpoint to mark an opportunity as won");

            // Stages must move forward one step at a time, in the exact order declared in
            // OpportunityStageEnum (Prospecting -> Qualification -> ProposalSent -> Negotiation).
            // ClosedLost is the one exception - it's reachable from any open stage, not just "the next one".
            if (request.Stage != OpportunityStageEnum.ClosedLost)
            {
                var expectedNextStage = (OpportunityStageEnum)((int)opportunity.Stage + 1);
                if (request.Stage != expectedNextStage)
                {
                    return ServiceResult<bool>.Fail(
                        $"Stages must be updated in order. Expected next stage after {opportunity.Stage} is {expectedNextStage}, but received {request.Stage}.");
                }
            }

            if (request.Stage == OpportunityStageEnum.ClosedLost && string.IsNullOrWhiteSpace(request.LostReason))
                return ServiceResult<bool>.Fail("A reason is required when marking an opportunity as Closed Lost.");

            opportunity.Stage = request.Stage;
            opportunity.LostReason = request.Stage == OpportunityStageEnum.ClosedLost ? request.LostReason : null; ;

            if(request.Stage == OpportunityStageEnum.ClosedLost)
            {
                opportunity.ActualClosedDate = DateTime.UtcNow;
                opportunity.Probability = 0;
            }

            _uow.Repository<Opportunity>().Update(opportunity);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0
            ? ServiceResult<bool>.Ok(true, "Opportunity stage updated.")
            : ServiceResult<bool>.Fail("Failed to update opportunity stage.");
        }

        public async Task<ServiceResult<CloseOpportunityAsWonResponse>> CloseOpportunityAsWon(int id)
        {
            var opportunity = await _uow.Repository<Opportunity>().GetOneAsync(o => o.Id == id);
            
            if(opportunity == null)
                return ServiceResult<CloseOpportunityAsWonResponse>.NotFound("Opportunity not found");

            if (isClosed(opportunity.Stage))
                return ServiceResult<CloseOpportunityAsWonResponse>.Fail($"This opportunity is already {opportunity.Stage}");

            var sale = new Sale
            {
                Amount = opportunity.Amount,
                Currancy = opportunity.Currancy,
                OpportunityId = opportunity.Id,
                CustomerId = opportunity.CustomerId,
                AssignedAgentId = opportunity.AssignedAgentId,
                SaleDate = DateTime.UtcNow,
            };
            
            await _uow.Repository<Sale>().CreateAsync(sale);

            opportunity.Stage = OpportunityStageEnum.ClosedWon;
            opportunity.ActualClosedDate = DateTime.UtcNow;
            opportunity.Probability = 100;

            _uow.Repository<Opportunity>().Update(opportunity);

            var affected = await _uow.SaveChangesAsync();

            if (affected == 0)
                return ServiceResult<CloseOpportunityAsWonResponse>.Fail("Failed to close opportunity as won.");

            return ServiceResult<CloseOpportunityAsWonResponse>.Ok(
                new CloseOpportunityAsWonResponse
                {
                    SaleId = sale.Id,
                    OpportunityId = opportunity.Id,
                    SaleDate = sale.SaleDate

                }, "Opportunity closed as won, sale created.");
        }
        public async Task<ServiceResult<bool>> DeleteOpportunity(int id)
        {
            var opportunity = await _uow.Repository<Opportunity>().GetOneAsync(o => o.Id == id);

            if (opportunity == null)
                return ServiceResult<bool>.NotFound("Opportunity not found");

            opportunity.EntityStatus = EntityStatusEnum.InActive;
            _uow.Repository<Opportunity>().Update(opportunity);
            var affected = await _uow.SaveChangesAsync();

            return affected > 0 ?
                ServiceResult<bool>.Ok(true, "Opportunity deleted successfully.")
                : ServiceResult<bool>.Fail("Failed to delete opportunity");
        }
    }
}
