using AutoMapper;

using PawPay.Application.Models.Responses;
using PawPay.Domain.Models;

namespace PawPay.Application.Mapping;

public class ResponseMapper : Profile
{
    public ResponseMapper()
    {
        CreateMap<BankApiResponse.ValuteResponse, Valute>();
    }
}