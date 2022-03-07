using FireplaceApi.Core.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FireplaceApi.Core.Tools
{
    public class CoreContractResolver : DefaultContractResolver
    {
        public static readonly CoreContractResolver Instance = new();
        private readonly List<string> _sensitiveNames = new()
        {
            "token",
            "password"
        };

        public CoreContractResolver()
        {
            NamingStrategy = new SnakeCaseNamingStrategy();
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty jsonProperty = base.CreateProperty(member, memberSerialization);

            if (member.GetCustomAttribute<System.Text.Json.Serialization.JsonIgnoreAttribute>() != null)
                jsonProperty.Ignored = true;

            PropertyInfo propertyInfo = member as PropertyInfo;

            if (propertyInfo.PropertyType == typeof(string) && IsPropertySensitive(jsonProperty, member))
            {
                jsonProperty.ValueProvider = new SensitiveValueProvider(propertyInfo);
            }

            return jsonProperty;
        }

        private bool IsPropertySensitive(JsonProperty jsonProperty, MemberInfo member)
        {
            var propertyName = jsonProperty.PropertyName;

            if (propertyName.Contains("type", StringComparison.OrdinalIgnoreCase))
                return false;

            if (member.GetCustomAttribute<SensitiveAttribute>() != null)
                return true;

            foreach (var sensitiveName in _sensitiveNames)
            {
                if (propertyName.Contains(sensitiveName, StringComparison.OrdinalIgnoreCase))
                    return true;
            }

            return false;
        }

        public class SensitiveValueProvider : IValueProvider
        {
            private PropertyInfo _targetProperty;

            public SensitiveValueProvider(PropertyInfo targetProperty)
            {
                _targetProperty = targetProperty;
            }

            public void SetValue(object target, object value)
            {
                _targetProperty.SetValue(target, value);
            }

            public object GetValue(object target)
            {
                string value = (string)_targetProperty.GetValue(target);
                if (value != null)
                    value = new string('*', Math.Min(value.Length, 10));
                return value;
            }
        }
    }
}
