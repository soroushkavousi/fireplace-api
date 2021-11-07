using FireplaceApi.Core.Tools;
using NLog;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Tools
{
    public static class IdGenerator
    {
        private static readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private static readonly Random _random = new Random();
        private static readonly string[] b64pad = { "", "ERROR", "==", "=" };

        public static async Task<ulong> GenerateNewIdAsync(Func<ulong, Task<bool>> doesIdExistAsync = null)
        {
            var min = (ulong)Math.Pow(2, 10);
            var max = ulong.MaxValue - 1;
            ulong id; bool idIsValid;
            do
            {
                id = Utils.GenerateRandomUlongNumber(min, max);
                idIsValid = await IsIdValid(id, doesIdExistAsync);
            }
            while (!idIsValid);
            return id;
        }

        private static async Task<bool> IsIdValid(ulong id, Func<ulong, Task<bool>> doesIdExistAsync = null)
        {
            var encodedId = EncodeId(id);
            if (!IsEncodedIdFormatValid(encodedId))
                return false;

            if (doesIdExistAsync != null && await doesIdExistAsync(id))
                return false;

            return true;
        }

        public static bool IsEncodedIdFormatValid(string encodedId)
        {
            if (Regexes.EncodedId.IsMatch(encodedId))
                return false;

            return true;
        }

        public static ulong DecodeId(string encodedId)
        {
            var b64 = encodedId.Replace('-', '+').Replace('_', '/') + b64pad[encodedId.Length % 4];
            return BitConverter.ToUInt64(Convert.FromBase64String(b64), 0);
        }

        public static ulong? DecodeIdOrDefault(string encodedId)
        {
            if (string.IsNullOrWhiteSpace(encodedId))
                return default;
            return DecodeId(encodedId);
        }

        public static string EncodeId(ulong id)
        {
            var bytes = BitConverter.GetBytes(id);
            var b64 = Convert.ToBase64String(bytes);
            b64 = b64.Replace('+', '-').Replace('/', '_').Remove(b64.Length - 1);
            return b64;
        }
    }
}

namespace FireplaceApi.Core.Extensions
{
    public static class IdGeneratorExtensions
    {
        public static string Encode(this ulong id)
            => IdGenerator.EncodeId(id);

        public static ulong Decode(this string encodedId)
            => IdGenerator.DecodeId(encodedId);

        public static ulong? DecodeIdOrDefault(this string encodedId)
            => IdGenerator.DecodeIdOrDefault(encodedId);
    }
}
