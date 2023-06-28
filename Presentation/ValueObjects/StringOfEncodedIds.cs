using FireplaceApi.Domain.Errors;
using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Presentation.ValueObjects;

public record StringOfEncodedIds(string Value, FieldName Field)
{
    private readonly List<ulong> _ids = DecodeValue(Value, Field);

    public List<ulong> Ids => _ids;

    private static List<ulong> DecodeValue(string value, FieldName field)
    {
        if (string.IsNullOrWhiteSpace(value))
            return new();

        return value
            .Split(',')
            .Select(encodedId => encodedId.ToId(field))
            .ToList();
    }
}

public static class StringOfEncodedIdsExtensions
{
    public static List<ulong> ToIds(this string stringOfEncodedIds, FieldName Field)
        => new StringOfEncodedIds(stringOfEncodedIds, Field).Ids;
}
