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

        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddMediatR(c => c.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
    }
}