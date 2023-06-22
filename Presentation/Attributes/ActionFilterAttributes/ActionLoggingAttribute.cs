using FireplaceApi.Application.Exceptions;
using FireplaceApi.Application.Extensions;
using FireplaceApi.Presentation.Tools;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace FireplaceApi.Presentation.Attributes;

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
        var actionName = context.ActionDescriptor.To<ControllerActionDescriptor>().ActionName;
        context.HttpContext.Items.Add(Constants.ActionNameKey, actionName);
        _logger.LogAppInformation(message: actionName, title: "ACTION_INPUT", parameters: actionInput);
        _sw = Stopwatch.StartNew();
    }

    public override void OnActionExecuted(ActionExecutedContext context)
    {
        var title = "ACTION_OUTPUT";
        var actionName = context.ActionDescriptor.To<ControllerActionDescriptor>().ActionName;
        var message = actionName;
        if (context.Exception != null)
        {
            message += $" | {context.Exception.GetType().Name}";
            if (context.Exception is ApiException apiException)
                message += $" | {apiException.ErrorType} | {apiException.ErrorField}";
            else
                message += $" | {context.Exception.Message}";
            _logger.LogAppInformation(title: title, message: message, sw: _sw);
            return;
        }

        if (context.Result == null)
        {
            message += " | No Result!";
            _logger.LogAppInformation(title: title, message: message, sw: _sw);
            return;
        }

        switch (context.Result)
        {
            case OkResult:
                message += " | Ok Result!";
                _logger.LogAppInformation(title: title, message: message, sw: _sw);
                break;
            case RedirectResult redirectResult:
                message += " | Redirect Result!";
                var redirectUrl = redirectResult.Url.Length > 25 ? $"{redirectResult.Url[..25]}..." : redirectResult.Url;
                _logger.LogAppInformation(title: title, message: message, parameters: new { redirectUrl }, sw: _sw);
                break;
            case ObjectResult objectResult:
                _logger.LogAppInformation(title: title, message: message, parameters: objectResult.Value, sw: _sw);
                break;
            default:
                message += " | Unknown Output => {context.Result}";
                _logger.LogAppInformation(title: title, message: message, sw: _sw);
                break;
        }
    }
}
