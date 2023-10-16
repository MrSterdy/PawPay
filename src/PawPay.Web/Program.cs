using MediatR;
using Microsoft.AspNetCore.Mvc;
using PawPay.Application;
using PawPay.Application.Commands;
using PawPay.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);

var app = builder.Build();

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