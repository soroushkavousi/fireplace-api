using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace FireplaceApi.Application.Extensions;

public static class OtherExtensions
{
    public static TV GetValue<TK, TV>(this IDictionary<TK, TV> dict, TK key, TV defaultValue = default) =>
        dict.TryGetValue(key, out TV value) ? value : defaultValue;

    public static bool IsNullOrEmpty<T>(this IEnumerable<T> enumerable)
    {
        if (enumerable == null || !enumerable.Any())
        {
            return true;
        }
        return false;
    }

    public static List<T> CopyOrDefault<T>(this List<T> list)
    {
        if (list == null)
            return new List<T>();

        var copy = new List<T>(list);
        return copy;
    }

    public static bool IsLocalIpAddress(this IPAddress ip) => ip.ToString().IsLocalIpAddress();
}
