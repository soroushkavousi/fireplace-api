using FireplaceApi.Api.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace FireplaceApi.Api.Attributes
{
    public class InputCookieParametersInjectorAttribute : ActionFilterAttribute
    {
        private readonly string _key = "inputCookieParameters";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var doesActionHaveInputCookieParameters = context.ActionDescriptor.Parameters
                .Any(parameterDescriptor => parameterDescriptor.Name == _key);
            if (doesActionHaveInputCookieParameters == false)
                return;
            context.ActionArguments[_key] = context.HttpContext.GetInputCookieParameters();
        }
    }
}
