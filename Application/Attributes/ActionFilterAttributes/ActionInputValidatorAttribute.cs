using FireplaceApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace FireplaceApi.Application.Attributes
{
    public class ActionInputValidatorAttribute : ActionFilterAttribute
    {
        private readonly ILogger<ActionInputValidatorAttribute> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ActionInputValidatorAttribute(ILogger<ActionInputValidatorAttribute> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var actionInputs = context.ActionArguments.Values.OfType<IValidator>().ToList();
            actionInputs.ForEach(input => input.Validate(_serviceProvider));
        }
    }
}
