using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Tools
{
    public static class Regexes
    {
        public static Regex Password { get; } = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$");
        public static Regex PasswordMinLength { get; } = new Regex(@"^.{8,}$");
        public static Regex PasswordMaxLength { get; } = new Regex(@"^.{0,25}$");
        public static Regex PasswordAnUppercaseLetter { get; } = new Regex(@"[A-Z]");
        public static Regex PasswordALowercaseLetter { get; } = new Regex(@"[a-z]");
        public static Regex PasswordValidCharacters { get; } = new Regex(@"^[A-Za-z\d!""#$%&'()*+,-./:;<=>?@[\]^_`{|}~]+$");
        public static Regex PasswordANumber { get; } = new Regex(@"\d");
        public static Regex MobileNumber { get; } = new Regex(@"^09\d{9}$");
        public static Regex EmailAddress { get; } = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$");
        public static Regex Username { get; } = new Regex(@"^(?=.{6,25}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._]+(?<![_.])$");
        public static Regex UsernameMinLength { get; } = new Regex(@"^.{6,}$");
        public static Regex UsernameMaxLength { get; } = new Regex(@"^.{0,25}$");
        public static Regex UsernameStart { get; } = new Regex(@"^(?![_.]).*$");
        public static Regex UsernameEnd { get; } = new Regex(@"^.*(?<![_.])$");
        public static Regex UsernameSafeConsecutives { get; } = new Regex(@"^(?!.*[_.]{2}).*$");
        public static Regex UsernameValidCharacters { get; } = new Regex(@"^[a-zA-Z0-9_.]+$");
        public static Regex AuthorizationHeaderValue { get; } = new Regex(@"Bearer\s+(.+)", RegexOptions.IgnoreCase);
        public static Regex AccessTokenValue { get; } = new Regex(@"([\d|a-f]{32})", RegexOptions.IgnoreCase);
        public static Regex FirstName { get; } = new Regex(@"^(?=.*\S).*$");
        public static Regex LastName { get; } = new Regex(@"^(?=.*\S).*$");
        public static Regex ErrorClientMessage { get; } = new Regex(@"^(?=.*\S).*$");

    }
}
