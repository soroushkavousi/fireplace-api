using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace FireplaceApi.Api.Attributes
{
    public class RequestingUserInjectorAttribute : ActionFilterAttribute
    {
        private readonly string _key = "requestingUser";

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var doesActionHaveRequestingUser = context.ActionDescriptor.Parameters
                .Any(parameterDescriptor => parameterDescriptor.Name == _key);
            if (doesActionHaveRequestingUser == false)
                return;
            context.ActionArguments[_key] = context.HttpContext.Items.GetValue(Constants.RequestingUserKey);
        }
    }
}
