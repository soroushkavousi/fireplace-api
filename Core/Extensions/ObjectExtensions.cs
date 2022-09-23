using FireplaceApi.Core.Tools.NewtonsoftSerializer;
using Newtonsoft.Json;

namespace FireplaceApi.Core.Extensions
{
    public static class ObjectExtensions
    {
        public static string ToJson(this object obj, bool secureSensitive = false)
        {
            if (obj == null)
                return null;

            if (secureSensitive)
                return JsonConvert.SerializeObject(obj, SensitiveJsonSerializerSettings.Instance);
            else
                return JsonConvert.SerializeObject(obj, CoreJsonSerializerSettings.Instance);
        }

        public static DestinationType To<DestinationType>(this object obj)
        {
            if (obj == null)
                return default;
            return (DestinationType)obj;
        }
    }
}
