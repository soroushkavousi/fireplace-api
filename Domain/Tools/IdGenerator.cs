using SimpleBase;
using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Tools
{
    public static partial class IdGenerator
    {
        private static readonly ulong _min = (ulong)Math.Pow(2, 10);
        private static readonly ulong _max = ulong.MaxValue - 1;
        private static readonly Regex _encodedIdWrongRepetitionRegex = EncodedIdWrongRepetitionRegex();
        private static readonly Regex _encodedIdWrongCharactersRegex = EncodedIdWrongCharactersRegex();
        private static readonly Regex _encodedIdValidCharactersRegex = EncodedIdValidCharactersRegex();

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

        public static string IdEncode(this ulong? id)
        {
            if (id == null)
                return null;

            return id.Value.IdEncode();
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
            if (!encodedId.IsEncodedIdFormatValid())
                return default;

            return encodedId.IdDecode();
        }

        private static async Task<bool> IsIdValid(this ulong id,
            Func<ulong, Task<bool>> doesIdExistAsync = null)
        {
            // To filter 10-length encoded IDs
            if (id % 256 < 6)
                return false;

            var encodedId = id.IdEncode();
            if (!encodedId.IsEncodedIdFormatValid())
                return false;

            if (doesIdExistAsync != null && await doesIdExistAsync(id))
                return false;

            return true;
        }

        public static bool IsEncodedIdFormatValid(this string encodedId)
        {
            if (string.IsNullOrWhiteSpace(encodedId))
                return false;

            encodedId = encodedId.Trim();
            if (encodedId.Length != 11)
                return false;

            // To filter encoded IDs which has three same characters in a row
            if (_encodedIdWrongRepetitionRegex.IsMatch(encodedId))
                return false;

            if (_encodedIdWrongCharactersRegex.IsMatch(encodedId))
                return false;

            if (_encodedIdValidCharactersRegex.IsMatch(encodedId) == false)
                return false;

            return true;
        }

        [GeneratedRegex("(\\S)\\1{2}")]
        private static partial Regex EncodedIdWrongRepetitionRegex();

        [GeneratedRegex("([0OlI])")]
        private static partial Regex EncodedIdWrongCharactersRegex();

        [GeneratedRegex("^[a-zA-Z0-9]+$")]
        private static partial Regex EncodedIdValidCharactersRegex();
    }
}
