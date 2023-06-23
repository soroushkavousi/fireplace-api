using Newtonsoft.Json.Serialization;
using System.Text.Json;

namespace FireplaceApi.Application.Common;

public class KebabCaseNamingPolicy : JsonNamingPolicy
{
    private readonly SnakeCaseNamingStrategy _newtonsoftSnakeCaseNamingStrategy = new();

    public static KebabCaseNamingPolicy Instance { get; } = new KebabCaseNamingPolicy();

    public override string ConvertName(string name)
    {
        return _newtonsoftSnakeCaseNamingStrategy.GetPropertyName(name, false).Replace("_", "-");
    }
}
