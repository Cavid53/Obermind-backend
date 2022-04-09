using Domain.Common;
using Microsoft.Extensions.DependencyInjection;
using Service.Account;
using Service.OrderItems;
using Service.Orders;
using System.Reflection;

namespace Service
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddServiceLayer(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderItemService, OrderItemService>();

            return services;
        }
    }
}
