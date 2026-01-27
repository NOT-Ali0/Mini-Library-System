using LoanSystem.Application;
using LoanSystem.Infrastructure;

namespace LoanSystem.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddAppDI(this IServiceCollection services)
        {
            services.AddInfrastructureDI()
                .AddApplicationDI();
            return services;
        }
    }
}
