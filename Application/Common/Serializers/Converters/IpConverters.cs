using System;
using System.Net;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FireplaceApi.Application.Common;

public class IPAddressConverter : JsonConverter<IPAddress>
{
    public override IPAddress Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return IPAddress.Parse(value);
    }

    public override void Write(Utf8JsonWriter writer, IPAddress ipAddress,
        JsonSerializerOptions options)
        => writer.WriteStringValue(ipAddress?.ToString());
}

public class IPEndPointConverter : JsonConverter<IPEndPoint>
{
    public override IPEndPoint Read(ref Utf8JsonReader reader, Type typeToConvert,
        JsonSerializerOptions options)
    {
        var value = reader.GetString();
        if (string.IsNullOrWhiteSpace(value))
            return null;

        return IPEndPoint.Parse(value);
    }

    public override void Write(Utf8JsonWriter writer, IPEndPoint ipEndPoint,
        JsonSerializerOptions options)
        => writer.WriteStringValue(ipEndPoint?.ToString());
}
