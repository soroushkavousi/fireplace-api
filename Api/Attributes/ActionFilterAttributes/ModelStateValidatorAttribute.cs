//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using GamingCommunityApi.Api.Exceptions;
//using GamingCommunityApi.Api.Extensions;
//using Microsoft.AspNetCore.Mvc.Controllers;
//using System.Diagnostics;
//using GamingCommunityApi.Api.Controllers.Parameters.UserParameters;
//using Microsoft.Extensions.Logging;
//using GamingCommunityApi.Api.Enums;

//namespace GamingCommunityApi.Api.Attributes.ActionFilterAttributes
//{
//    public class ModelStateValidatorAttribute : ActionFilterAttribute
//    {
//        public override void OnActionExecuting(ActionExecutingContext context)
//        {
//            if (!context.ModelState.IsValid)
//            {
//                throw new ApiException($"#ModelState | {new { context.ActionArguments }.ToJson()} | [{string.Join(",", context.ModelState.GetErrorMessages())}]");
//            }
//        }
//    }
//}
