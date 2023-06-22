using FireplaceApi.Application.Tools.NewtonsoftSerializer;
using Newtonsoft.Json;

namespace FireplaceApi.Application.Extensions;

public static class ObjectExtensions
{
    public static string ToJson(this object obj, bool ignoreSensitiveLimit = false)
    {
        if (obj == null)
            return null;

        var jsonSerializerSettings = ignoreSensitiveLimit ? CoreJsonSerializerSettings.Instance : SensitiveJsonSerializerSettings.Instance;
        return JsonConvert.SerializeObject(obj, jsonSerializerSettings);
    }

    public static DestinationType To<DestinationType>(this object obj)
    {
        if (obj == null)
            return default;
        return (DestinationType)obj;
    }
}
