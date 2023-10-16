using Microsoft.OpenApi.Models;

namespace PawPay.Web;

public static class ConfigureServices
{
    public static void AddWebServices(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PawPay API", Version = "v1", Contact = new OpenApiContact
                {
                    Name = "-> ЗАЦЕНИ МОЙ ГИТХАБ, ТАМ СКОРО ИМБА ПРОЕКТ БУДЕТ ПО РАСПИСАНИЯМ И ДЗ <-",
                    Url = new Uri("https://github.com/MrSterdy")
                }
            });
        });
    }
}