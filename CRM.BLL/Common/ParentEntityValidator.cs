using CRM.DAL.Models;
using CRM.DAL.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Common
{
    public static class ParentEntityValidator
    {
        public static async Task<(bool IsValid, string? Error)> ValidateExactlyOneParent(
        IUnitOfWork uow, int? leadId, int? customerId, int? opportunityId)
        {
            var setCount = new[] { leadId, customerId, opportunityId }.Count(x => x.HasValue);
            if (setCount != 1)
                return (false, "Exactly one of LeadId, CustomerId, or OpportunityId must be provided.");

            if (leadId.HasValue)
            {
                var lead = await uow.Repository<Lead>().GetOneAsync(l => l.Id ==  leadId);
                if (lead is null)
                    return (false, "Lead not found");
            }
            else if (customerId.HasValue)
            {
                var customer = uow.Repository<Customer>().GetOneAsync(c => c.Id == customerId);
                if (customer is null) return (false, "Customer not found");
            }
            else if (opportunityId.HasValue)
            {
                var opportunity = uow.Repository<Opportunity>().GetOneAsync(o => o.Id == opportunityId);
                if (opportunity is null) return (false, "Opportunity not found");
            }

            return (true, null);
        }
    }
}
