using System;

namespace FireplaceApi.Presentation.Swagger;

[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
public class SwaggerEnumAttribute : Attribute
{
    public Type Type { get; set; }
}
