using System;

namespace FireplaceApi.Application.Attributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class ProducesCsrfTokenAttribute : Attribute
    {

    }
}
