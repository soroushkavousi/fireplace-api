using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using FireplaceApi.Api.Tools;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Api.Tools
{
    public class ActionResponseExampleProvider : IOperationFilter
    {
        private readonly ILogger<ActionResponseExampleProvider> _logger;
        private readonly IServiceProvider _serviceProvider;

        public ActionResponseExampleProvider(ILogger<ActionResponseExampleProvider> logger, 
            IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var bodyParametersType = ExtractResponseBodyParametersType(context);
            if (bodyParametersType == null)
                return;
            IOpenApiAny example = ExtractExample(bodyParametersType, context.MethodInfo.Name);
            if(bodyParametersType.Name.Contains("Error"))
            {
                switch(example)
                {
                    case OpenApiArray errorExampleList:
                        for (int i = 0; i < errorExampleList.Count; i++)
                        {
                            var errorExample = errorExampleList[i].To<OpenApiObject>();
                            if (errorExample.ContainsKey("message") == false)
                                errorExample = TypeExampleProvider.FillErrorExampleFromName(errorExample, _serviceProvider, _logger).GetAwaiter().GetResult();
                            errorExampleList[i] = errorExample;
                        }
                        example = errorExampleList;
                        break;

                    case OpenApiObject errorExample:
                        if (errorExample.ContainsKey("message") == false)
                            errorExample = TypeExampleProvider.FillErrorExampleFromName(errorExample, _serviceProvider, _logger).GetAwaiter().GetResult();
                        example = errorExample;
                        break;
                }
            }
            var successResponse = operation.Responses["200"];
            successResponse.Content["application/json"].Example = example;
        }

        public Type ExtractResponseBodyParametersType(OperationFilterContext context)
        {
            var taskType = context.MethodInfo.ReturnType;
            var resultActionType = taskType.GetGenericArguments()[0];
            if (resultActionType.IsGenericType == false)
                return null;
            var bodyParametersType = resultActionType.GetGenericArguments()[0];
            if (bodyParametersType.IsEnumerable())
            {
                bodyParametersType = bodyParametersType.GetGenericArguments()[0];
            }
            return bodyParametersType;
        }

        public IOpenApiAny ExtractExample(Type type, string actionName)
        {
            if (type.IsGenericType)
                type = type.GenericTypeArguments[0];
            var exampleProperty = type.GetProperty(Constants.ActionExamplesPropertyName, BindingFlags.Public | BindingFlags.Static);
            if (exampleProperty == null)
            {
                throw new ApiException(ErrorName.INTERNAL_SERVER, $"Type {type} doesn't have example for action {actionName}");
            }

            var actionExamples = exampleProperty.GetValue(null)?.To<Dictionary<string, IOpenApiAny>>();
            if (actionExamples == null || actionExamples.Count == 0 || actionExamples.ContainsKey(actionName) == false)
                throw new ApiException(ErrorName.INTERNAL_SERVER, $"Type {type} doesn't have example for action {actionName}");

            return actionExamples[actionName];
        }
    }
}
