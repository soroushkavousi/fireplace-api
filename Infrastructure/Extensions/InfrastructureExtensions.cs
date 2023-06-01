using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Infrastructure.Extensions;

public static class InfrastructureExtensions
{
    public static List<decimal> ToDecimals(this List<ulong> ilongs)
    {
        if (ilongs == null)
            return null;

        return ilongs.Select(ul => (decimal)ul).ToList();
    }

    public static List<ulong> ToUlongs(this List<decimal> decimals)
    {
        if (decimals == null)
            return null;

        return decimals.Select(d => (ulong)d).ToList();
    }
}
