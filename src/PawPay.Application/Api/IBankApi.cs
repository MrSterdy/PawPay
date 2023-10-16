using PawPay.Application.Models.Responses;

using Refit;

namespace PawPay.Application.Api;

public interface IBankApi
{
    [Get("/daily_json.js")]
    Task<BankApiResponse> GetValute();
}