using FireplaceApi.Application.Exceptions;
using FireplaceApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
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
            ValidateRequestBodyIsNotMissing(context);
            var actionInputs = context.ActionArguments.Values.OfType<IValidator>().ToList();
            actionInputs.ForEach(input => input.Validate(_serviceProvider));
        }

        private void ValidateRequestBodyIsNotMissing(ActionExecutingContext context)
        {
            var bodyParameterDescriptor = context.ActionDescriptor.Parameters.SingleOrDefault(p => p.BindingInfo.BindingSource == BindingSource.Body);
            if (bodyParameterDescriptor == null)
                return;

            if (!context.ActionArguments.ContainsKey(bodyParameterDescriptor.Name))
                throw new RequestBodyMissingFieldException();
        }
    }
}
