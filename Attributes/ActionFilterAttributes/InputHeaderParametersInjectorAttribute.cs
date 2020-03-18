using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Exceptions;
using GamingCommunityApi.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Diagnostics;
using GamingCommunityApi.Controllers.Parameters.UserParameters;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Enums;
using GamingCommunityApi.Tools;

namespace GamingCommunityApi.Attributes.ActionFilterAttributes
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
