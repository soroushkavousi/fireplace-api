using FireplaceApi.Domain.Tools.NewtonsoftSerializer;
using Newtonsoft.Json;

namespace FireplaceApi.Domain.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj, bool ignoreSensitiveLimit = false)
        {
            if (obj == null)
                return null;

            if (ignoreSensitiveLimit)
                return JsonConvert.SerializeObject(obj, CoreJsonSerializerSettings.Instance);
            else
                return JsonConvert.SerializeObject(obj, SensitiveJsonSerializerSettings.Instance);
        }

        public static DestinationType To<DestinationType>(this object obj)
        {
            if (obj == null)
                return default;
            return (DestinationType)obj;
        }
    }
}
