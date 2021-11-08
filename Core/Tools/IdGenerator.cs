using FireplaceApi.Core.Tools;
using SimpleBase;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Tools
{
    public static class IdGenerator
    {
        private static readonly ulong _min = (ulong)Math.Pow(2, 10);
        private static readonly ulong _max = ulong.MaxValue - 1;
        private static readonly Regex _encodedIdRegex = new Regex(@"(\S)\1{2}");

        public static async Task<ulong> GenerateNewIdAsync(Func<ulong, Task<bool>> doesIdExistAsync = null)
        {
            ulong id; bool idIsValid;
            do
            {
                id = Utils.GenerateRandomUlongNumber(_min, _max);
                idIsValid = await IsIdValid(id, doesIdExistAsync);
            }
            while (!idIsValid);
            return id;
        }

        private static async Task<bool> IsIdValid(ulong id, Func<ulong, Task<bool>> doesIdExistAsync = null)
        {
            if (id % 256 < 6)
                return false;

            var encodedId = EncodeId(id);
            if (!IsEncodedIdFormatValid(encodedId))
                return false;

            if (doesIdExistAsync != null && await doesIdExistAsync(id))
                return false;

            return true;
        }

        public static bool IsEncodedIdFormatValid(string encodedId)
        {
            if (_encodedIdRegex.IsMatch(encodedId))
                return false;

            return true;
        }

        public static string EncodeId(ulong id)
        {
            var bytes = BitConverter.GetBytes(id);
            var encodedId = Base58.Bitcoin.Encode(bytes);
            return encodedId;
        }

        public static ulong DecodeId(string encodedId)
        {
            ReadOnlySpan<byte> idBytes = Base58.Bitcoin.Decode(encodedId);
            var id = BitConverter.ToUInt64(idBytes);
            return id;
        }

        public static ulong? DecodeIdOrDefault(string encodedId)
        {
            if (string.IsNullOrWhiteSpace(encodedId))
                return default;
            return DecodeId(encodedId);
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
