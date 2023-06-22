using FireplaceApi.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace FireplaceApi.Presentation.Attributes;

public class InputCookieDtoInjectorAttribute : ActionFilterAttribute
{
    private readonly string _key = "inputCookieDto";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var doesActionHaveInputCookieDto = context.ActionDescriptor.Parameters
            .Any(parameterDescriptor => parameterDescriptor.Name == _key);
        if (doesActionHaveInputCookieDto == false)
            return;
        context.ActionArguments[_key] = context.HttpContext.GetInputCookieDto();
    }
}
