using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

using PawPay.Application.Api;

using Refit;

namespace PawPay.Application;

public static class ConfigureServices
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services
            .AddRefitClient<IBankApi>()
            .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://www.cbr-xml-daily.ru/"));

        services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetCallingAssembly()));
        services.AddValidatorsFromAssembly(Assembly.GetCallingAssembly());
    }
}