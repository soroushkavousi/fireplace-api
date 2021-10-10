using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace FireplaceApi.Api.Attributes
{
    public class InputHeaderParametersInjectorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var doesActionHaveInputHeaderParameters = context.ActionDescriptor.Parameters
                .Any(parameterDescriptor => parameterDescriptor.Name == Constants.InputHeaderParametersActionArgumentKey);
            if (doesActionHaveInputHeaderParameters == false)
                return;
            context.ActionArguments[Constants.InputHeaderParametersActionArgumentKey] = context.HttpContext.Items[Constants.ControllerInputHeaderParametersKey];
        }
    }
}
