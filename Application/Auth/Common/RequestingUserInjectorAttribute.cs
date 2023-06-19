using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace FireplaceApi.Application.Auth;

public class RequestingUserInjectorAttribute : ActionFilterAttribute
{
    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var requestingUserParameter = context.ActionDescriptor.Parameters
            .SingleOrDefault(p => p.ParameterType == typeof(RequestingUser));
        if (requestingUserParameter == null)
            return;
        context.ActionArguments[requestingUserParameter.Name] = context.HttpContext.GetRequestingUser();
    }
}
