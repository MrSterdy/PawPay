using MediatR;

using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using PawPay.Application;
using PawPay.Application.Commands;
using PawPay.Domain.Models;
using PawPay.Infrastructure;
using PawPay.Web;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddWebServices();

var app = builder.Build();

app.UseHttpsRedirection();

app.UseExceptionHandler(c => c.Run(async context =>
{
    object? response;

    var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
    if (exception is BadHttpRequestException badException)
    {
        response = new { error = exception.Message };
        context.Response.StatusCode = badException.StatusCode;
    }
    else
    {
        response = new { error = "Произошла непредвиденная ошибка" };
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
    }

    await context.Response.WriteAsJsonAsync(response);
}));

app.MapPost("/convert/rub-to-usd",
    async ([FromBody] ConvertRublesToDollarsCommand request, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.IsT0 ? Results.Ok(result.AsT0) : Results.BadRequest(result.AsT1);
    })
    .WithOpenApi(c => new OpenApiOperation(c) { Summary = "Конвертировать рубли в доллары" })
    .Produces<ConvertResult>()
    .Produces<ConvertError>(StatusCodes.Status400BadRequest);
app.MapPost("/convert/usd-to-rub",
        async ([FromBody] ConvertDollarsToRublesCommand request, [FromServices] IMediator mediator,
            CancellationToken cancellationToken) =>
        {
            var result = await mediator.Send(request, cancellationToken);

            return result.IsT0 ? Results.Ok(result.AsT0) : Results.BadRequest(result.AsT1);
        })
    .WithOpenApi(c => new OpenApiOperation(c) { Summary = "Конвертировать доллары в рубли" })
    .Produces<ConvertResult>()
    .Produces<ConvertError>(StatusCodes.Status400BadRequest);

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "PawPay API");
    c.RoutePrefix = string.Empty;
});

app.Run();