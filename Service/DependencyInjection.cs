using Microsoft.Extensions.DependencyInjection;
using Service.Account;
using System.Reflection;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IAccountService, AccountService>();

            return services;
        }
    }
}
