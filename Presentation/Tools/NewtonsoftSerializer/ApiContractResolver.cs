using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace FireplaceApi.Presentation.Tools;

public class ApiContractResolver : DefaultContractResolver
{
    public static readonly ApiContractResolver Instance = new();

    protected ApiContractResolver()
    {
        NamingStrategy = new SnakeCaseNamingStrategy();
    }

    protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
    {
        JsonProperty property = base.CreateProperty(member, memberSerialization);

        if (member.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() != null)
            property.Ignored = true;

        return property;
    }
}
