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
                idIsValid = await id.IsIdValid(doesIdExistAsync);
            }
            while (!idIsValid);
            return id;
        }

        //public static async Task<ulong> GenerateNewIdAsync(Func<ulong, Task<bool>> doesIdExistAsync = null)
        //{
        //    ulong id; bool idIsValid;
        //    do
        //    {
        //        id = Utils.GenerateRandomUlongNumber(_min, _max);
        //        idIsValid = await id.IsIdValid(doesIdExistAsync);
        //    }
        //    while (!idIsValid);
        //    return id;
        //}

        private static async Task<bool> IsIdValid(this ulong id,
            Func<ulong, Task<bool>> doesIdExistAsync = null)
        {
            if (id % 256 < 6)
                return false;

            var encodedId = IdEncode(id);
            if (!IsEncodedIdFormatValid(encodedId))
                return false;

            if (doesIdExistAsync != null && await doesIdExistAsync(id))
                return false;

            return true;
        }

        public static bool IsEncodedIdFormatValid(this string encodedId)
        {
            if (string.IsNullOrWhiteSpace(encodedId))
                return false;

            if (encodedId.Length != 11)
                return false;

            if (_encodedIdRegex.IsMatch(encodedId))
                return false;

            return true;
        }

        public static string IdEncode(this ulong id)
        {
            var bytes = BitConverter.GetBytes(id);
            var encodedId = Base58.Bitcoin.Encode(bytes);
            return encodedId;
        }

        public static ulong IdDecode(this string encodedId)
        {
            ReadOnlySpan<byte> idBytes = Base58.Bitcoin.Decode(encodedId);
            var id = BitConverter.ToUInt64(idBytes);
            return id;
        }

        public static ulong? IdDecodeOrDefault(this string encodedId)
        {
            if (!IsEncodedIdFormatValid(encodedId))
                return default;

            return IdDecode(encodedId);
        }
    }
}
