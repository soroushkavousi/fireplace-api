using FireplaceApi.Domain.Errors;
using System.Text.RegularExpressions;

namespace FireplaceApi.Domain.Communities;

public partial record CommunityName(string Value)
{
    private readonly string _value = ValidateValue(Value);

    public string Value
    {
        get => _value;
        init => _value = ValidateValue(value);
    }

    private static string ValidateValue(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new CommunityNameMissingFieldException();

        if (MinLengthRegex.IsMatch(value) == false)
            throw new CommunityNameInvalidFormatException(value,
                "The community name doesn't have the minimum length!");

        if (MaxLengthRegex.IsMatch(value) == false)
            throw new CommunityNameInvalidFormatException(value,
                "The community name exceeds the maximum length!");

        if (ValidCharactersRegex.IsMatch(value) == false)
            throw new CommunityNameInvalidFormatException(value,
                "The community name has invalid characters!");

        return value;
    }

    private static Regex MinLengthRegex { get; } = GetMinLengthRegex();
    private static Regex MaxLengthRegex { get; } = GetMaxLengthRegex();
    private static Regex ValidCharactersRegex { get; } = GetValidCharactersRegex();

    [GeneratedRegex("^.{3,}$")]
    private static partial Regex GetMinLengthRegex();

    [GeneratedRegex("^.{0,32}$")]
    private static partial Regex GetMaxLengthRegex();

    [GeneratedRegex("^[a-zA-Z0-9_-]+$")]
    private static partial Regex GetValidCharactersRegex();

    public override string ToString()
        => Value;
}
