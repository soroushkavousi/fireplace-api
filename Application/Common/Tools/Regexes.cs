using System.Text.RegularExpressions;

namespace FireplaceApi.Application.Common;

public static partial class Regexes
{
    public static Regex Password { get; } = PasswordRegex();
    public static Regex PasswordMinLength { get; } = PasswordMinLengthRegex();
    public static Regex PasswordMaxLength { get; } = PasswordMaxLengthRegex();
    public static Regex PasswordAnUppercaseLetter { get; } = PasswordAnUppercaseLetterRegex();
    public static Regex PasswordALowercaseLetter { get; } = PasswordALowercaseLetterRegex();
    public static Regex PasswordASpecialLetter { get; } = PasswordASpecialLetterRegex();
    public static Regex PasswordValidCharacters { get; } = PasswordValidCharactersRegex();
    public static Regex PasswordANumber { get; } = PasswordANumberRegex();
    public static Regex MobileNumber { get; } = MobileNumberRegex();
    public static Regex EmailAddress { get; } = EmailAddressRegex();

    public static Regex AuthorizationHeaderValue { get; } = AuthorizationHeaderValueRegex();
    public static Regex AccessTokenValue { get; } = AccessTokenValueRegex();
    public static Regex TextWithWhitespace { get; } = TextWithWhitespaceRegex();

    [GeneratedRegex("^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$")]
    private static partial Regex PasswordRegex();

    [GeneratedRegex("^.{8,}$")]
    private static partial Regex PasswordMinLengthRegex();

    [GeneratedRegex("^.{0,128}$")]
    private static partial Regex PasswordMaxLengthRegex();

    [GeneratedRegex("[A-Z]")]
    private static partial Regex PasswordAnUppercaseLetterRegex();

    [GeneratedRegex("[a-z]")]
    private static partial Regex PasswordALowercaseLetterRegex();

    [GeneratedRegex("[!#$%&'()*+,.:;<=>?@^_`{|}~\\+\\-\\\"\\\\\\/\\[\\]]")]
    private static partial Regex PasswordASpecialLetterRegex();

    [GeneratedRegex("^[A-Za-z\\d!#$%&'()*+,.:;<=>?@^_`{|}~\\+\\-\\\"\\\\\\/\\[\\]]+$")]
    private static partial Regex PasswordValidCharactersRegex();

    [GeneratedRegex("\\d")]
    private static partial Regex PasswordANumberRegex();

    [GeneratedRegex("^09\\d{9}$")]
    private static partial Regex MobileNumberRegex();

    [GeneratedRegex("^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\\.[a-zA-Z]{2,}$")]
    private static partial Regex EmailAddressRegex();



    [GeneratedRegex("Bearer\\s+(.+)", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex AuthorizationHeaderValueRegex();

    [GeneratedRegex("([\\d|a-f]{32})", RegexOptions.IgnoreCase, "en-US")]
    private static partial Regex AccessTokenValueRegex();

    [GeneratedRegex("^\\S*$")]
    private static partial Regex TextWithWhitespaceRegex();

}
