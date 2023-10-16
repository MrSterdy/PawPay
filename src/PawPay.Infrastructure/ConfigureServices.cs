using Microsoft.Extensions.DependencyInjection;

using PawPay.Application.Services;
using PawPay.Infrastructure.Services;

namespace PawPay.Infrastructure;

public static class ConfigureServices
{
    public static void AddInfrastructureServices(this IServiceCollection services)
    {
        services.AddScoped<IConverter, Converter>();
    }
}