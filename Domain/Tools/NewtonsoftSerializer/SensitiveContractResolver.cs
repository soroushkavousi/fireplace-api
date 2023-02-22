using FireplaceApi.Domain.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace FireplaceApi.Domain.Tools
{
    public class SensitiveContractResolver : CoreContractResolver
    {
        public static new readonly SensitiveContractResolver Instance = new();

        public SensitiveContractResolver() : base() { }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);
            if (jsonProperty.Ignored)
                return jsonProperty;

            if (member.GetCustomAttribute<SensitiveAttribute>() != null)
            {
                var propertyInfo = member as PropertyInfo;
                jsonProperty.PropertyType = typeof(string);
                jsonProperty.ValueProvider = new SensitiveValueProvider(propertyInfo);
            }

            return jsonProperty;
        }

        public class SensitiveValueProvider : IValueProvider
        {
            private readonly PropertyInfo _propertyInfo;
            private readonly string _value;

            public SensitiveValueProvider(PropertyInfo propertyInfo)
            {
                _propertyInfo = propertyInfo;
                _value = $"(SENSITIVE_{propertyInfo.PropertyType.Name.ToUpper()})";
            }

            public void SetValue(object target, object value)
            {

            }

            public object GetValue(object target)
            {
                var value = _propertyInfo.GetValue(target);
                if (value == null)
                    return null;

                return _value;
            }
        }
    }
}
