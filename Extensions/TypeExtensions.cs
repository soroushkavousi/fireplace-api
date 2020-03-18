﻿using Microsoft.OpenApi.Any;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GamingCommunityApi.Extensions
{
    public static class TypeExtensions
    {
        public static OpenApiObject ExtractExample(this Type type)
        {
            var exampleProperty = type.GetProperty(Constants.ExamplePropertyName, BindingFlags.Public | BindingFlags.Static);
            if (exampleProperty == null)
            {
                throw new ApiException(ErrorName.INTERNAL_SERVER, $"Type {type} 'Example' is empty!");
            }

            var test = exampleProperty.GetValue(null);
            var example = exampleProperty.GetValue(null).To<OpenApiObject>();
            if (example == null || example.Count == 0)
                throw new ApiException(ErrorName.INTERNAL_SERVER, $"Type {type} 'Example' is empty!");

            return example;
        }

        public static bool IsEnumerable(this Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IEnumerable<>)));
        }
    }
}
