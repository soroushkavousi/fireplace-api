using System.Text.RegularExpressions;

namespace FireplaceApi.Domain.Tools
{
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
        public static Regex Username { get; } = UsernameRegex();
        public static Regex UsernameMinLength { get; } = UsernameMinLengthRegex();
        public static Regex UsernameMaxLength { get; } = UsernameMaxLengthRegex();
        public static Regex UsernameStart { get; } = UsernameStartRegex();
        public static Regex UsernameEnd { get; } = UsernameEndRegex();
        public static Regex UsernameSafeConsecutives { get; } = UsernameSafeConsecutivesRegex();
        public static Regex UsernameValidCharacters { get; } = UsernameValidCharactersRegex();
        public static Regex AuthorizationHeaderValue { get; } = AuthorizationHeaderValueRegex();
        public static Regex AccessTokenValue { get; } = AccessTokenValueRegex();
        public static Regex ErrorClientMessage { get; } = ErrorClientMessageRegex();
        public static Regex TextWithWhitespace { get; } = TextWithWhitespaceRegex();
        public static Regex CommunityNameMinLength { get; } = CommunityNameMinLengthRegex();
        public static Regex CommunityNameMaxLength { get; } = CommunityNameMaxLengthRegex();
        public static Regex CommunityNameValidCharacters { get; } = CommunityNameValidCharactersRegex();

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

        [GeneratedRegex("^(?=.{6,25}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$")]
        private static partial Regex UsernameRegex();

        [GeneratedRegex("^.{6,}$")]
        private static partial Regex UsernameMinLengthRegex();

        [GeneratedRegex("^.{0,30}$")]
        private static partial Regex UsernameMaxLengthRegex();

        [GeneratedRegex("^(?![_]).*$")]
        private static partial Regex UsernameStartRegex();

        [GeneratedRegex("^.*(?<![_])$")]
        private static partial Regex UsernameEndRegex();

        [GeneratedRegex("^(?!.*[_]{2}).*$")]
        private static partial Regex UsernameSafeConsecutivesRegex();

        [GeneratedRegex("^[a-zA-Z0-9_]+$")]
        private static partial Regex UsernameValidCharactersRegex();

        [GeneratedRegex("Bearer\\s+(.+)", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex AuthorizationHeaderValueRegex();

        [GeneratedRegex("([\\d|a-f]{32})", RegexOptions.IgnoreCase, "en-US")]
        private static partial Regex AccessTokenValueRegex();

        [GeneratedRegex("^(?=.*\\S).*$")]
        private static partial Regex ErrorClientMessageRegex();

        [GeneratedRegex("^\\S*$")]
        private static partial Regex TextWithWhitespaceRegex();

        [GeneratedRegex("^.{3,}$")]
        private static partial Regex CommunityNameMinLengthRegex();

        [GeneratedRegex("^.{0,32}$")]
        private static partial Regex CommunityNameMaxLengthRegex();

        [GeneratedRegex("^[a-zA-Z0-9_-]+$")]
        private static partial Regex CommunityNameValidCharactersRegex();
    }
}
