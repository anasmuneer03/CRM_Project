using CRM.BLL.Service.Authentication;
using CRM.BLL.Service.Customers;
using CRM.BLL.Service.Email;
using CRM.BLL.Service.Leads;
using CRM.BLL.Service.Opportunities;
using CRM.DAL.Repository;
using CRM.DAL.Utils;

namespace CRM.PL.Extensions
{
    public static class ApplicationExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IUnitOfWork,UnitOfWork>();
            services.AddScoped<ISeedData, RoleSeedData>();
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ILeadService, LeadService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IOpportunityService, OpportunityService>();  
            return services;
        }
    }
}
