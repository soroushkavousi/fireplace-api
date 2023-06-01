using FireplaceApi.Application.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace FireplaceApi.Application.Attributes;

public class InputHeaderParametersInjectorAttribute : ActionFilterAttribute
{
    private readonly string _key = "inputHeaderParameters";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var doesActionHaveInputHeaderParameters = context.ActionDescriptor.Parameters
            .Any(parameterDescriptor => parameterDescriptor.Name == _key);
        if (doesActionHaveInputHeaderParameters == false)
            return;
        context.ActionArguments[_key] = context.HttpContext.GetInputHeaderParameters();
    }
}
