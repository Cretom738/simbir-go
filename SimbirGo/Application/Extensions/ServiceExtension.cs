using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Extensions
{
    public static class ServiceExtension
    {
        public static void AddServices(this IServiceCollection services)
        {
            services.AddScoped<IAccountAdminService, AccountAdminService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<IPaymentService, PaymentService>();
            services.AddScoped<IRentAdminService, RentAdminService>();
            services.AddScoped<IRentService, RentService>();
            services.AddScoped<ITransportAdminService, TransportAdminService>();
            services.AddScoped<ITransportService, TransportService>();
        }
    }
}
