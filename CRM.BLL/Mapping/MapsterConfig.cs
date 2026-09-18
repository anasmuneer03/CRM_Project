using CRM.DAL.DTO.Response.Customers;
using CRM.DAL.DTO.Response.Leads;
using CRM.DAL.DTO.Response.Opportunities;
using CRM.DAL.Models;
using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.BLL.Mapping
{
    public static class MapsterConfig
    {
        public static void MapsterConfigRegister()
        {
            TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

            TypeAdapterConfig<Lead, LeadResponse>.NewConfig()
                .Map(dest => dest.AssignedAgentName, src => src.AssignedAgent != null ? src.AssignedAgent.FullName : null);

            TypeAdapterConfig<Customer, CustomerResponse>.NewConfig()
                .Map(dest => dest.AssignedAgentName, src => src.AssignedAgent != null ? src.AssignedAgent.FullName : null);

            TypeAdapterConfig<Opportunity, OpportunityResponse>.NewConfig()
                .Map(dest => dest.AssignedAgentName, src => src.AssignedAgent != null ? src.AssignedAgent.FullName : null)
                .Map(dest=> dest.CustomerName, src => src.Customer != null ? src.Customer.FullName : null);
        }
    }
}
