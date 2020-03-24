using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Api.Extensions;
using Microsoft.AspNetCore.Mvc.Controllers;
using System.Diagnostics;
using GamingCommunityApi.Api.Controllers.Parameters.UserParameters;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Api.Tools;
using GamingCommunityApi.Core.Extensions;

namespace GamingCommunityApi.Api.Attributes.ActionFilterAttributes
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
