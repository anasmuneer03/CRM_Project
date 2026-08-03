using CRM.BLL.Services.Leads;
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
            return services;
        }
    }
}
