using System.Text.Json.Serialization;

namespace PawPay.Application.Models.Responses;

public class BankApiResponse
{
    public class ValuteResponse
    {
        [JsonPropertyName("ID")]
        public required string Id { get; set; }

        public required string NumCode { get; set; }
        public required string CharCode { get; set; }

        public required int Nominal { get; set; }

        public required string Name { get; set; }
        public required float Value { get; set; }
    }

    public required Dictionary<string, ValuteResponse> Valute { get; set; }
}