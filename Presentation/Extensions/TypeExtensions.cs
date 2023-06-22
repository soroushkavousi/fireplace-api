using FireplaceApi.Application.Exceptions;
using FireplaceApi.Application.Extensions;
using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FireplaceApi.Presentation.Extensions;

public static class TypeExtensions
{
    public static OpenApiObject ExtractExample(this Type type)
    {
        var exampleProperty = type.GetProperty(Tools.Constants.ExamplePropertyName, BindingFlags.Public | BindingFlags.Static);
        if (exampleProperty == null)
            throw new InternalServerException("The type has not [Example] property!", new { type });

        var example = exampleProperty.GetValue(null).To<OpenApiObject>();
        if (example == null)
            throw new InternalServerException("The type [Example] property is empty!", new { type });

        return example;
    }

    public static bool IsEnumerable(this Type type)
    {
        return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)));
    }
}
