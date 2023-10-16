using System.Text;
using System.Text.Json;

using AutoMapper;

using Microsoft.Extensions.Caching.Distributed;

using PawPay.Application.Api;
using PawPay.Application.Models.Responses;
using PawPay.Application.Services;
using PawPay.Domain.Models;

namespace PawPay.Infrastructure.Services;

public class Converter : IConverter
{
    private const string CacheKey = "value";

    private readonly IBankApi _api;

    private readonly IDistributedCache _cache;

    private readonly IMapper _mapper;

    public Converter(IBankApi api, IMapper mapper, IDistributedCache cache)
    {
        _api = api;
        _mapper = mapper;
        _cache = cache;
    }

    private async Task<Valute> GetValute(string name, CancellationToken cancellationToken = default)
    {
        BankApiResponse? response;

        var cacheBytes = await _cache.GetAsync(CacheKey, cancellationToken);
        if ((cacheBytes?.Length ?? 0) > 0)
        {
            response = JsonSerializer.Deserialize<BankApiResponse>(Encoding.UTF8.GetString(cacheBytes!));
        }
        else
        {
            response = await _api.GetValute();

            var expiration = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
            };

            await _cache.SetAsync(CacheKey, JsonSerializer.SerializeToUtf8Bytes(response), expiration, cancellationToken);
        }

        var rawValute = response!.Valute[name];
        rawValute.Value /= rawValute.Nominal;

        return _mapper.Map<Valute>(rawValute);
    }

    public async Task<(float Result, Valute Valute)> ConvertRublesToDollars(float rubles, CancellationToken cancellationToken = default)
    {
        var valute = await GetValute("USD", cancellationToken);

        return (rubles / valute.Value, valute);
    }

    public async Task<(float Result, Valute Valute)> ConvertDollarsToRubles(float dollars, CancellationToken cancellationToken = default)
    {
        var valute = await GetValute("USD", cancellationToken);

        return (dollars * valute.Value, valute);
    }
}