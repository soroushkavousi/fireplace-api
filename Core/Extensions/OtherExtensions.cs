using Microsoft.Extensions.Logging;
using NLog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Extensions
{
    public static class OtherExtensions
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();

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
    }
}
