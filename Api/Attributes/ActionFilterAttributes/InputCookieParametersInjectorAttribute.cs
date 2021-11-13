using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace FireplaceApi.Api.Attributes
{
    public class InputCookieParametersInjectorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var doesActionHaveInputCookieParameters = context.ActionDescriptor.Parameters
                .Any(parameterDescriptor => parameterDescriptor.Name == Constants.InputCookieParametersActionArgumentKey);
            if (doesActionHaveInputCookieParameters == false)
                return;
            context.ActionArguments[Constants.InputCookieParametersActionArgumentKey] = context.HttpContext.Items[Constants.InputCookieParametersKey];
        }
    }
}
