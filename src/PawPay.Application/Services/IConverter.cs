using PawPay.Domain.Models;

namespace PawPay.Application.Services;

public interface IConverter
{
    Task<(float Result, Valute Valute)> ConvertRublesToDollars(float rubles,
        CancellationToken cancellationToken = default);

    Task<(float Result, Valute Valute)> ConvertDollarsToRubles(float dollars,
        CancellationToken cancellationToken = default);
}