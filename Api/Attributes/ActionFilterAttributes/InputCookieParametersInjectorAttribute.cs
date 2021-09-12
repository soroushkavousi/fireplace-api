using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Api.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Diagnostics;
using Microsoft.Extensions.Logging;
using FireplaceApi.Api.Tools;

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
            context.ActionArguments[Constants.InputCookieParametersActionArgumentKey] = context.HttpContext.Items[Constants.ControllerInputCookieParametersKey];
        }
    }
}
