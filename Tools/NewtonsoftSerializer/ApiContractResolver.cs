using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GamingCommunityApi.Tools.NewtonsoftSerializer
{
    public class ApiContractResolver : DefaultContractResolver
    {
        public static readonly ApiContractResolver Instance = new ApiContractResolver();

        public ApiContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (member.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() != null)
                property.Ignored = true;

            //if (property.DeclaringType == typeof(Employee) &&
            //      property.PropertyName == "Manager")
            //{
            //    property.ShouldSerialize = instance =>
            //    {
            //        Employee e = (Employee)instance;
            //        return e.Manager != e;
            //    };
            //}

            return property;
        }
    }
}
