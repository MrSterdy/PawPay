using FluentValidation;

using MediatR;

using OneOf;

using PawPay.Application.Services;
using PawPay.Domain.Models;

namespace PawPay.Application.Commands;

public class ConvertDollarsToRublesCommand : IRequest<OneOf<ConvertResult, ConvertError>>
{
    public float Dollars { get; set; }
}

public class
    ConvertDollarsToRublesQuery : IRequestHandler<ConvertDollarsToRublesCommand, OneOf<ConvertResult, ConvertError>>
{
    private readonly IValidator<ConvertDollarsToRublesCommand> _validator;

    private readonly IConverter _converter;

    public ConvertDollarsToRublesQuery(IValidator<ConvertDollarsToRublesCommand> validator, IConverter converter)
    {
        _validator = validator;
        _converter = converter;
    }

    public async Task<OneOf<ConvertResult, ConvertError>> Handle(ConvertDollarsToRublesCommand request,
        CancellationToken cancellationToken)
    {
        var validationResult = await _validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid)
        {
            return new ConvertError(validationResult.Errors[0].ErrorMessage);
        }

        var convertResult = await _converter.ConvertDollarsToRubles(request.Dollars, cancellationToken);

        return new ConvertResult(request.Dollars, convertResult.Result, convertResult.Valute);
    }
}

public class ConvertDollarsToRublesValidator : AbstractValidator<ConvertDollarsToRublesCommand>
{
    public ConvertDollarsToRublesValidator()
    {
        RuleFor(c => c.Dollars).GreaterThanOrEqualTo(0).WithMessage("Вводимое значение не должно быть меньше 0");
    }
}