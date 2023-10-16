using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using PawPay.Application.Services;
using PawPay.Infrastructure.Services;

namespace PawPay.Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IConverter, Converter>();

        services.AddStackExchangeRedisCache(o =>
        {
            o.Configuration = configuration.GetConnectionString("Redis");
        });
    }
}