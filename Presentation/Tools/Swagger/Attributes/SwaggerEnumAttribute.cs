using System;

namespace FireplaceApi.Presentation.Tools;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SwaggerEnumAttribute : Attribute
{
    public Type Type { get; set; }
}
