using FireplaceApi.Domain.Errors;
using System.Text.RegularExpressions;

namespace FireplaceApi.Domain.Users;

public partial record Username(string Value)
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
            throw new UsernameMissingFieldException();

        if (MinLengthRegex.IsMatch(value) == false)
            throw new UsernameInvalidFormatException(value,
                "The username doesn't have the minimum length!");

        if (MaxLengthRegex.IsMatch(value) == false)
            throw new UsernameInvalidFormatException(value,
                "The username exceeds the maximum length!");

        if (StartRegex.IsMatch(value) == false)
            throw new UsernameInvalidFormatException(value,
                "The username has wrong starts!");

        if (EndRegex.IsMatch(value) == false)
            throw new UsernameInvalidFormatException(value,
            "The username has wrong end!");

        if (SafeConsecutivesRegex.IsMatch(value) == false)
            throw new UsernameInvalidFormatException(value,
            "The username has invalid consecutive!");

        if (ValidCharactersRegex.IsMatch(value) == false)
            throw new UsernameInvalidFormatException(value,
                "The username has invalid characters!");

        return value;
    }

    private static Regex ValueRegex { get; } = GetValueRegex();
    private static Regex MinLengthRegex { get; } = GetMinLengthRegex();
    private static Regex MaxLengthRegex { get; } = GetMaxLengthRegex();
    private static Regex StartRegex { get; } = GetStartRegex();
    private static Regex EndRegex { get; } = GetEndRegex();
    private static Regex SafeConsecutivesRegex { get; } = GetSafeConsecutivesRegex();
    private static Regex ValidCharactersRegex { get; } = GetValidCharactersRegex();

    [GeneratedRegex("^(?=.{6,25}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$")]
    private static partial Regex GetValueRegex();

    [GeneratedRegex("^.{6,}$")]
    private static partial Regex GetMinLengthRegex();

    [GeneratedRegex("^.{0,30}$")]
    private static partial Regex GetMaxLengthRegex();

    [GeneratedRegex("^(?![_]).*$")]
    private static partial Regex GetStartRegex();

    [GeneratedRegex("^.*(?<![_])$")]
    private static partial Regex GetEndRegex();

    [GeneratedRegex("^(?!.*[_]{2}).*$")]
    private static partial Regex GetSafeConsecutivesRegex();

    [GeneratedRegex("^[a-zA-Z0-9_]+$")]
    private static partial Regex GetValidCharactersRegex();
}
