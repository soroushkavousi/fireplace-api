using System;

namespace FireplaceApi.Application.Tools;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SwaggerEnumAttribute : Attribute
{
    public Type Type { get; set; }
}
