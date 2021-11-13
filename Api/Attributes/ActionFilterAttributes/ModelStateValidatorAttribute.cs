//using Microsoft.AspNetCore.Mvc.Filters;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using FireplaceApi.Api.Exceptions;
//using FireplaceApi.Api.Extensions;
//using Microsoft.AspNetCore.Mvc.Controllers;
//using System.Diagnostics;
//using FireplaceApi.Api.s.Parameters.UserParameters;
//using Microsoft.Extensions.Logging;
//using FireplaceApi.Api.Enums;

//namespace FireplaceApi.Api.Attributes
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
