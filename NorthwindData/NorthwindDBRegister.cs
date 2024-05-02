using Microsoft.Extensions.DependencyInjection;

namespace NorthwindData
{
    public static class NorthwindDBRegister
    {
        public static IServiceCollection AddNorthwindServices(this IServiceCollection services)
            => services.AddScoped<INorthwindConnection, NorthwindSQLServerConnection>()
                       .AddScoped<INorthwindDataService, NorthwindDataService>();
    }
}
