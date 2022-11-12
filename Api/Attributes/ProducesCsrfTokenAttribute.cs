using System;

namespace FireplaceApi.Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ProducesCsrfTokenAttribute : Attribute
    {

    }
}
