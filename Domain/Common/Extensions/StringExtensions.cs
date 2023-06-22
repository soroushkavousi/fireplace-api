using System.Security.Cryptography;
using System.Text;

namespace FireplaceApi.Domain.Common;

public static class StringExtensions
{
    public static string ToHash(this string content)
    {
        var hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(content));
        var hash = new StringBuilder();
        foreach (byte b in hashBytes)
            hash.Append(b.ToString("X2"));
        return hash.ToString();
    }
}
