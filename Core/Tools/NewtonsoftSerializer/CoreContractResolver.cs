using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace FireplaceApi.Core.Tools
{
    public class CoreContractResolver : DefaultContractResolver
    {
        public static readonly CoreContractResolver Instance = new();

        public CoreContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);

            if (member.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() != null)
                jsonProperty.Ignored = true;

            return jsonProperty;
        }
    }
}
