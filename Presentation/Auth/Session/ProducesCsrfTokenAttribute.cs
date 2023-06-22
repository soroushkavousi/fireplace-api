using System;

namespace FireplaceApi.Presentation.Auth;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProducesCsrfTokenAttribute : Attribute
{

}
