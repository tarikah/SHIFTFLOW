using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ShiftFlow.Infrastructure.Contexts;

namespace ShiftFlow.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {

            return services
                .AddPersistence(configuration);
        }

        private static IServiceCollection AddPersistence(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddDbContext<ShiftFlowDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("Development"))
                        .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
                        .EnableSensitiveDataLogging()
                        .EnableDetailedErrors()
            );
            return services;
        }
    }
}
