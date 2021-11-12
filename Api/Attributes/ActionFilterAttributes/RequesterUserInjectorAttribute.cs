using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace FireplaceApi.Api.Attributes
{
    public class RequestingUserInjectorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var doesActionHaveRequestingUser = context.ActionDescriptor.Parameters
                .Any(parameterDescriptor => parameterDescriptor.Name == Constants.RequestingUserActionArgumentKey);
            if (doesActionHaveRequestingUser == false)
                return;
            context.ActionArguments[Constants.RequestingUserActionArgumentKey] =
                context.HttpContext.Items.GetValue(Constants.RequestingUserKey);
        }
    }
}
