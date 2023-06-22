using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace FireplaceApi.Presentation.Tools;

public class UlongRouteConstraint : IRouteConstraint
{
    public static readonly string Name = "ulong";

    public bool Match(HttpContext httpContext, IRouter route, string routeKey,
        RouteValueDictionary values, RouteDirection routeDirection)
    {
        if (values.TryGetValue(routeKey, out object value))
        {
            if (ulong.TryParse(value.ToString(), out ulong ulongNumber))
            {
                return true;
            }
        }
        return false;
    }
}
