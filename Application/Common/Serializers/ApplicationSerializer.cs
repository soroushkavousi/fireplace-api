using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace FireplaceApi.Application.Common;

public static class ApplicationSerializer
{
    public static readonly JsonSerializerOptions ApplicationOptions = new();
    public static readonly JsonSerializerOptions SensitiveOptions = new();

    static ApplicationSerializer()
    {
        ApplicationOptions.AddApplicationOptions();
        SensitiveOptions.AddApplicationOptions();
        SensitiveOptions.AddSensitiveOptions();
    }

    public static void AddApplicationOptions(this JsonSerializerOptions options)
    {
        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
        options.AddApplicationConverters();
    }

    public static void AddApplicationConverters(this JsonSerializerOptions options)
    {
        options.Converters.Add(new JsonStringEnumConverter());
        options.Converters.Add(new IPAddressConverter());
        options.Converters.Add(new IPEndPointConverter());

        options.Converters.Add(new UsernameJsonConverter());
    }

    public static void AddSensitiveOptions(this JsonSerializerOptions options)
    {
        options.TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { Modifiers.SensitiveModifier }
        };
    }

    public static string ToJson(this object obj, bool ignoreSensitiveLimit = false)
    {
        if (obj == null)
            return null;

        var serializerOptions = ignoreSensitiveLimit ?
            ApplicationOptions : SensitiveOptions;
        return JsonSerializer.Serialize(obj, serializerOptions);
    }

    public static T FromJson<T>(this string json)
    {
        if (json == null)
            return default;
        return JsonSerializer.Deserialize<T>(json, ApplicationOptions);
    }

    public static string ToSnakeCase(this string str) =>
        SnakeCaseNamingPolicy.Instance.ConvertName(str);

    public static string ToKebabCase(this string str) =>
        KebabCaseNamingPolicy.Instance.ConvertName(str);
}
