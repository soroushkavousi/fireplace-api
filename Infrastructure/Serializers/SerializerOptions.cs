using FireplaceApi.Infrastructure.Tools;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace FireplaceApi.Infrastructure.Serializers;

public static class SerializerOptions
{
    public static readonly JsonSerializerOptions Common = new();
    public static readonly JsonSerializerOptions Sensitive = new();

    static SerializerOptions()
    {
        Common.AddCommonOptions();
        Sensitive.AddCommonOptions();
        Sensitive.AddSensitiveOptions();
    }

    public static void AddCommonOptions(this JsonSerializerOptions options)
    {
        options.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        options.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
        options.AddConverters();
    }

    public static void AddConverters(this JsonSerializerOptions options)
    {
        options.Converters.Add(new JsonStringEnumConverter());

        var serverJsonConverters = Assembly.GetExecutingAssembly().GetTypes().Where(type =>
            type.IsClass
            && !type.IsAbstract
            && !type.IsInterface
            && type.IsLocal()
            && type.BaseType.IsGenericType
            && type.BaseType.GetGenericTypeDefinition() == typeof(JsonConverter<>)
            ).Select(type => (JsonConverter)Activator.CreateInstance(type))
            .ToList();

        serverJsonConverters.ForEach(options.Converters.Add);
    }

    public static void AddSensitiveOptions(this JsonSerializerOptions options)
    {
        options.TypeInfoResolver = new DefaultJsonTypeInfoResolver
        {
            Modifiers = { Modifiers.SensitiveModifier }
        };
    }
}
