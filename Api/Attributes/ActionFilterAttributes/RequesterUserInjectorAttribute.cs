using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Linq;

namespace FireplaceApi.Api.Attributes
{
    public class RequesterUserInjectorAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var doesActionHaveRequesterUser = context.ActionDescriptor.Parameters
                .Any(parameterDescriptor => parameterDescriptor.Name == Constants.RequesterUserActionArgumentKey);
            if (doesActionHaveRequesterUser == false)
                return;
            context.ActionArguments[Constants.RequesterUserActionArgumentKey] = context.HttpContext.Items.GetValue(Constants.RequesterUserKey);
        }
    }
}
