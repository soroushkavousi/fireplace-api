using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace FireplaceApi.Api.Tools
{
    /// <summary>
    /// For Serialization only.
    /// Emits properties in the specified order.
    /// </summary>
    public class JsonPropertyOrderConverter : JsonConverter<object>
    {
        delegate ExpandoObject SorterFunc(object value, bool ignoreNullValues);

        private static readonly ConcurrentDictionary<Type, SorterFunc> _sorters
            = new ConcurrentDictionary<Type, SorterFunc>();

        public override bool CanConvert(Type typeToConvert)
        {
            // Converter will not run if there is no custom order applied
            var sorter = _sorters.GetOrAdd(typeToConvert, CreateSorter);
            return sorter != null;
        }

        public override object Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotSupportedException();
        }

        public override void Write(Utf8JsonWriter writer, object value, JsonSerializerOptions options)
        {
            // Resolve the sorter.
            // It must exist here (see CanConvert).
            var sorter = _sorters.GetOrAdd(value.GetType(), CreateSorter);

            // Convert value to an ExpandoObject
            // with a certain property order
            var sortedValue = sorter(value, options.IgnoreNullValues);

            // Serialize the ExpandoObject
            JsonSerializer.Serialize(writer, (IDictionary<string, object>)sortedValue, options);
        }

        private SorterFunc CreateSorter(Type type)
        {
            // Get type properties ordered according to JsonPropertyOrder value
            var sortedProperties = type
                .GetProperties(BindingFlags.Instance | BindingFlags.Public)
                .Where(x => x.GetCustomAttribute<JsonIgnoreAttribute>(true) == null)
                .Select(x => new
                {
                    Info = x,
                    Name = x.GetCustomAttribute<JsonPropertyNameAttribute>(true)?.Name ?? x.Name,
                    Order = x.GetCustomAttribute<JsonPropertyOrderAttribute>(true)?.Order ?? 0,
                    IsExtensionData = x.GetCustomAttribute<JsonExtensionDataAttribute>(true) != null
                })
                .OrderBy(x => x.Order)
                .ToList();

            // If all properties have the same order,
            // there is no sense in explicit sorting
            if (!sortedProperties.Any(x => x.Order != 0))
            {
                return null;
            }

            // Return a function assigning property values
            // to an ExpandoObject in a specified order
            return new SorterFunc((src, ignoreNullValues) =>
            {
                IDictionary<string, object> dst = new ExpandoObject();

                var isExtensionDataProcessed = false;

                foreach (var prop in sortedProperties)
                {
                    var propValue = prop.Info.GetValue(src);

                    if (prop.IsExtensionData)
                    {
                        if (propValue is IDictionary extensionData)
                        {
                            if (isExtensionDataProcessed)
                            {
                                throw new InvalidOperationException($"The type '{src.GetType().FullName}' cannot have more than one property that has the attribute '{typeof(JsonExtensionDataAttribute).FullName}'.");
                            }

                            foreach (DictionaryEntry entry in extensionData)
                            {
                                dst.Add((string)entry.Key, entry.Value);
                            }
                        }

                        isExtensionDataProcessed = true;
                    }
                    else if (!ignoreNullValues || !(propValue is null))
                    {
                        dst.Add(prop.Name, propValue);
                    }
                }

                return (ExpandoObject)dst;
            });
        }
    }
}
