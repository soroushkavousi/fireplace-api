using FireplaceApi.Presentation.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace FireplaceApi.Presentation.Attributes;

public class InputHeaderDtoInjectorAttribute : ActionFilterAttribute
{
    private readonly string _key = "inputHeaderDto";

    public override void OnActionExecuting(ActionExecutingContext context)
    {
        var doesActionHaveInputHeaderDto = context.ActionDescriptor.Parameters
            .Any(parameterDescriptor => parameterDescriptor.Name == _key);
        if (doesActionHaveInputHeaderDto == false)
            return;
        context.ActionArguments[_key] = context.HttpContext.GetInputHeaderDto();
    }
}
