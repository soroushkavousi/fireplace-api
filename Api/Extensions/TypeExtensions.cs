using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FireplaceApi.Api.Extensions
{
    public static class TypeExtensions
    {
        public static OpenApiObject ExtractExample(this Type type)
        {
            var exampleProperty = type.GetProperty(Tools.Constants.ExamplePropertyName, BindingFlags.Public | BindingFlags.Static);
            if (exampleProperty == null)
            {
                throw new ApiException(ErrorName.INTERNAL_SERVER, $"Type {type} has no [Example] property!");
            }

            var test = exampleProperty.GetValue(null);
            var example = exampleProperty.GetValue(null).To<OpenApiObject>();
            if (example == null || example.Count == 0)
                throw new ApiException(ErrorName.INTERNAL_SERVER, $"Type {type} [Example] is empty!");

            return example;
        }

        public static bool IsEnumerable(this Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)));
        }
    }
}
