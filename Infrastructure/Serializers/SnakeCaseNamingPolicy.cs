using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace FireplaceApi.Infrastructure.Serializers;

public class SnakeCaseNamingPolicy : JsonNamingPolicy
{
    private readonly SnakeCaseNamingStrategy _newtonsoftSnakeCaseNamingStrategy = new();

    public static SnakeCaseNamingPolicy Instance { get; } = new SnakeCaseNamingPolicy();

    public override string ConvertName(string name)
    {
        return _newtonsoftSnakeCaseNamingStrategy.GetPropertyName(name, false);
    }
}
