using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Tools
{
    /// <summary>
    /// Sets a custom serialization order for a property.
    /// The default value is 0.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
    sealed class JsonPropertyOrderAttribute : Attribute
    {
        public int Order { get; }

        public JsonPropertyOrderAttribute(int order)
        {
            Order = order;
        }
    }
}
