using System.Net;
using MediatR;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using PawPay.Application;
using PawPay.Application.Commands;
using PawPay.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

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
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
    }

    await context.Response.WriteAsJsonAsync(response);
}));

app.UseHttpsRedirection();

app.MapPost("/convert/rub-to-usd",
    async ([FromBody] ConvertRublesToDollarsCommand request, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.IsT0 ? Results.Ok(result.AsT0) : Results.BadRequest(result.AsT1);
    });
app.MapPost("/convert/usd-to-rub",
    async ([FromBody] ConvertDollarsToRublesCommand request, [FromServices] IMediator mediator, CancellationToken cancellationToken) =>
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.IsT0 ? Results.Ok(result.AsT0) : Results.BadRequest(result.AsT1);
    });

app.Run();