using Domain.Common;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<AppUser, IdentityRole>()
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();

            services.AddDbContext<AppDbContext>(opt =>
            {
                opt.UseSqlServer(
                    configuration.GetConnectionString("ObermindDatabase")
                );
            });

            services.AddScoped<IAppDbContext>(provider => provider.GetService<AppDbContext>());

            return services;
        }
    }
}
