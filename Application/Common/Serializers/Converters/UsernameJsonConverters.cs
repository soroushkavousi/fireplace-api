using FireplaceApi.Domain.Users;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FireplaceApi.Application.Common;

public class UsernameJsonConverter : JsonConverter<Username>
{
    public override Username Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return new(value);
    }

    public override void Write(Utf8JsonWriter writer, Username username,
        JsonSerializerOptions options)
        => writer.WriteStringValue(username?.Value);
}
