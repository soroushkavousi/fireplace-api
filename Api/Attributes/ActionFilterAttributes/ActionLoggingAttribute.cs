using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace FireplaceApi.Api.Attributes
{
    public class ActionLoggingAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ActionLoggingAttribute> _logger;

        public ActionLoggingAttribute(ILogger<ActionLoggingAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionInput = context.ActionArguments;
            _logger.LogAppInformation(title: "ACTION_INPUT", parameters: actionInput);
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            var result = context.Result as ObjectResult;
            var actionOutput = result.Value;
            _logger.LogAppInformation(title: "ACTION_OUTPUT", parameters: actionOutput);
            base.OnActionExecuted(context);
        }
    }
}
