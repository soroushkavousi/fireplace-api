﻿using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace FireplaceApi.Application.Common;

public static class StringExtensions
{
    public static string FirstCharToUpper(this string str) =>
        str switch
        {
            null => throw new ArgumentNullException(nameof(str)),
            "" => throw new ArgumentException($"{nameof(str)} cannot be empty", nameof(str)),
            _ => str.First().ToString().ToUpper() + str[1..]
        };

    public static string Shuffle(this string str)
    {
        char[] array = str.ToCharArray();
        Random rng = new();
        int n = array.Length;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            var value = array[k];
            array[k] = array[n];
            array[n] = value;
        }
        return new string(array);
    }

    public static IPAddress ToIPAddress(this string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return null;
        return IPAddress.Parse(value);
    }

    public static bool IsMobileNumber(this string value)
    {
        bool isMobileNumber;
        var match = Regexes.MobileNumber.Match(value);
        isMobileNumber = match.Success;
        return isMobileNumber;
    }

    public static bool IsEmailAddress(this string value)
    {
        bool isEmailAddress;
        try
        {
            MailAddress address = new(value);
            isEmailAddress = address.Address == value;
        }
        catch (FormatException)
        {
            isEmailAddress = false;
        }
        return isEmailAddress;
    }

    public static string RemoveLineBreaks(this string str)
    {
        string result;
        result = str.Replace("\r\n", "").Replace("\n", "").Replace("\r", "");
        return Regex.Replace(result, @"\s+", " ");
    }

    public static string EscapeCurlyBrackets(this string str)
    {
        var result = str;
        result = result.Replace("{", "{{").Replace("}", "}}");
        return result;
    }

    public static string ToBase64UrlEncode(this string str)
    {
        var inputBytes = System.Text.Encoding.UTF8.GetBytes(str);
        // Special "url-safe" base64 encode.
        return Convert.ToBase64String(inputBytes)
          .Replace('+', '-')
          .Replace('/', '_')
          .Replace("=", "");
    }

    public static bool IsUrlStringValid(this string urlString, bool allSchemes = false)
    {
        if (!Uri.TryCreate(urlString, UriKind.Absolute, out Uri uriResult))
            return false;

        if (!allSchemes && uriResult.Scheme != Uri.UriSchemeHttp && uriResult.Scheme != Uri.UriSchemeHttps)
            return false;

        return true;
    }

    public static string ExtractFileNameWithoutExtension(this string path)
    {
        return Path.GetFileNameWithoutExtension(path);
    }

    // By Jan Slama
    public static bool IsLocalIpAddress(this string host)
    {
        try
        {
            // get host IP addresses
            IPAddress[] hostIPs = Dns.GetHostAddresses(host);
            // get local IP addresses
            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

            // test if any host IP equals to any local IP or to localhost
            foreach (IPAddress hostIP in hostIPs)
            {
                // is localhost
                if (IPAddress.IsLoopback(hostIP)) return true;
                // is local address
                foreach (IPAddress localIP in localIPs)
                {
                    if (hostIP.Equals(localIP)) return true;
                }
            }
        }
        catch { }
        return false;
    }
}
