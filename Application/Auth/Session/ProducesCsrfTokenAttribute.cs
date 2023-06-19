using System;

namespace FireplaceApi.Application.Auth;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class ProducesCsrfTokenAttribute : Attribute
{

}
