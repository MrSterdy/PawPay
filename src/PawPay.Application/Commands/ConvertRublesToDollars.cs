using FluentValidation;

using MediatR;

using OneOf;

using PawPay.Application.Services;
using PawPay.Domain.Models;

namespace PawPay.Application.Commands;

public class ConvertRublesToDollarsCommand : IRequest<OneOf<ConvertResult, ConvertError>>
{
    public float Rubles { get; set; }
}

public class
    ConvertRublesToDollarsQuery : IRequestHandler<ConvertRublesToDollarsCommand, OneOf<ConvertResult, ConvertError>>
{
    private readonly IValidator<ConvertRublesToDollarsCommand> _validator;

    private readonly IConverter _converter;

    public ConvertRublesToDollarsQuery(IValidator<ConvertRublesToDollarsCommand> validator, IConverter converter)
    {
        _validator = validator;
        _converter = converter;
    }

    public async Task<OneOf<ConvertResult, ConvertError>> Handle(ConvertRublesToDollarsCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ConvertError(validationResult.Errors[0].ErrorMessage);
        }

        var convertResult = await _converter.ConvertRublesToDollars(request.Rubles, cancellationToken);

        return new ConvertResult(request.Rubles, convertResult.Result, convertResult.Valute);
    }
}

public class ConvertRublesToDollarsValidator : AbstractValidator<ConvertRublesToDollarsCommand>
{
    public ConvertRublesToDollarsValidator()
    {
        RuleFor(c => c.Rubles).GreaterThanOrEqualTo(0).WithMessage("Вводимое значение не должно быть меньше 0");
    }
}
