using FireplaceApi.Infrastructure.Errors;
using SimpleBase;
using System;
using System.Text.RegularExpressions;

namespace FireplaceApi.Infrastructure.ValueObjects;

public partial record EncodedId
{
    private readonly string _value;
    private readonly ulong? _id;

    public string Value
    {
        get => _value;
        init => _value = ValidateValue(value);
    }
    public ulong Id
    {
        get => _id ?? DecodeEncodedId(Value);
        init => _id = value;
    }

    public EncodedId(string value)
    {
        _value = ValidateValue(value);
    }
    public EncodedId(ulong id)
    {
        _id = id;
        _value = EncodeId(id);
    }

    private static string EncodeId(ulong id)
    {
        var bytes = BitConverter.GetBytes(id);
        var encodedId = Base58.Bitcoin.Encode(bytes);
        ValidateValue(encodedId);
        return encodedId;
    }

    private static ulong DecodeEncodedId(string encodedId)
    {
        var idBytes = Base58.Bitcoin.Decode(encodedId);
        var id = BitConverter.ToUInt64(idBytes);
        return id;
    }

    private static string ValidateValue(string encodedId)
    {
        if (string.IsNullOrWhiteSpace(encodedId))
            throw new EncodedIdInvalidFormatException(encodedId);

        encodedId = encodedId.Trim();
        if (encodedId.Length != 11)
            throw new EncodedIdInvalidFormatException(encodedId);

        // To filter encoded IDs which has three same characters in a row
        if (_encodedIdWrongRepetitionRegex.IsMatch(encodedId))
            throw new EncodedIdInvalidFormatException(encodedId);

        if (_encodedIdWrongCharactersRegex.IsMatch(encodedId))
            throw new EncodedIdInvalidFormatException(encodedId);

        if (_encodedIdValidCharactersRegex.IsMatch(encodedId) == false)
            throw new EncodedIdInvalidFormatException(encodedId);

        return encodedId;
    }

    private static readonly Regex _encodedIdWrongRepetitionRegex = EncodedIdWrongRepetitionRegex();
    private static readonly Regex _encodedIdWrongCharactersRegex = EncodedIdWrongCharactersRegex();
    private static readonly Regex _encodedIdValidCharactersRegex = EncodedIdValidCharactersRegex();

    [GeneratedRegex("(\\S)\\1{2}")]
    private static partial Regex EncodedIdWrongRepetitionRegex();

    [GeneratedRegex("([0OlI])")]
    private static partial Regex EncodedIdWrongCharactersRegex();

    [GeneratedRegex("^[a-zA-Z0-9]+$")]
    private static partial Regex EncodedIdValidCharactersRegex();
}

public static class EncodedIdExtensions
{
    public static ulong IdDecode(this string value)
        => new EncodedId(value).Id;

    public static string IdEncode(this ulong id)
        => new EncodedId(id).Value;

    public static string IdEncode(this ulong? id)
        => id.HasValue ? id.Value.IdEncode() : null;
}
