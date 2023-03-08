using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FireplaceApi.Application.Attributes
{
    public class ActionLoggingAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ActionLoggingAttribute> _logger;
        private Stopwatch _sw;

        public ActionLoggingAttribute(ILogger<ActionLoggingAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionInput = context.ActionArguments;
            _logger.LogAppInformation(title: "ACTION_INPUT", parameters: actionInput);
            _sw = Stopwatch.StartNew();
        }

        public override void OnActionExecuted(ActionExecutedContext context)
        {
            if (context.Exception != null)
            {
                var message = context.Exception.GetType().Name;
                if (context.Exception is ApiException apiException)
                    message += $" | {apiException.ErrorType}";
                else
                    message += $" | {context.Exception.Message}";
                _logger.LogAppInformation(title: "ACTION_OUTPUT", message: message, sw: _sw);
                return;
            }

            if (context.Result == null)
            {
                _logger.LogAppInformation(title: "ACTION_OUTPUT", message: "No Result!", sw: _sw);
                return;
            }

            switch (context.Result)
            {
                case OkResult:
                    _logger.LogAppInformation(title: "ACTION_OUTPUT", message: "Ok Result!", sw: _sw);
                    break;
                case RedirectResult redirectResult:
                    var redirectUrl = redirectResult.Url.Length > 25 ? $"{redirectResult.Url[..25]}..." : redirectResult.Url;
                    _logger.LogAppInformation(title: "ACTION_OUTPUT", message: "Redirect Result!", parameters: new { redirectUrl }, sw: _sw);
                    break;
                case ObjectResult objectResult:
                    _logger.LogAppInformation(title: "ACTION_OUTPUT", parameters: objectResult.Value, sw: _sw);
                    break;
                default:
                    _logger.LogAppInformation(title: "ACTION_OUTPUT", message: $"Unknown Output => {context.Result}", sw: _sw);
                    break;
            }
        }
    }
}
