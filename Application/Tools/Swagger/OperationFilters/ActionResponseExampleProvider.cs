using FireplaceApi.Application.Extensions;
using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace FireplaceApi.Application.Tools;

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
        var bodyDtoType = ExtractResponseBodyDtoType(context);
        if (bodyDtoType == null)
            return;
        IOpenApiAny example = ExtractExample(bodyDtoType, context.MethodInfo.Name);
        if (bodyDtoType.Name.Contains("Error"))
        {
            switch (example)
            {
                case OpenApiArray errorExampleList:
                    for (int i = 0; i < errorExampleList.Count; i++)
                    {
                        var errorExample = errorExampleList[i].To<OpenApiObject>();
                        if (errorExample.ContainsKey("message") == false)
                            errorExample = TypeExampleProvider.FillErrorExample(errorExample, _serviceProvider, _logger).GetAwaiter().GetResult();
                        errorExampleList[i] = errorExample;
                    }
                    example = errorExampleList;
                    break;

                case OpenApiObject errorExample:
                    if (errorExample.ContainsKey("message") == false)
                        errorExample = TypeExampleProvider.FillErrorExample(errorExample, _serviceProvider, _logger).GetAwaiter().GetResult();
                    example = errorExample;
                    break;
            }
        }
        if (!operation.Responses.ContainsKey("200"))
            return;
        var successResponse = operation.Responses["200"];
        successResponse.Content["application/json"].Example = example;
    }

    public Type ExtractResponseBodyDtoType(OperationFilterContext context)
    {
        var taskType = context.MethodInfo.ReturnType;
        var resultActionType = taskType.GetGenericArguments()[0];
        if (resultActionType.IsGenericType == false)
            return null;
        var bodyDtoType = resultActionType.GetGenericArguments()[0];
        if (bodyDtoType.IsEnumerable())
        {
            bodyDtoType = bodyDtoType.GetGenericArguments()[0];
        }
        return bodyDtoType;
    }

    public IOpenApiAny ExtractExample(Type type, string actionName)
    {
        if (type.IsGenericType)
            type = type.GenericTypeArguments[0];
        var exampleProperty = type.GetProperty(Constants.ActionExamplesPropertyName, BindingFlags.Public | BindingFlags.Static);
        if (exampleProperty == null)
            throw new InternalServerException("Type doesn't have an example for the action!", new { type, actionName });

        var actionExamples = exampleProperty.GetValue(null)?.To<Dictionary<string, IOpenApiAny>>();
        if (actionExamples == null || actionExamples.Count == 0 || actionExamples.ContainsKey(actionName) == false)
            throw new InternalServerException("Type doesn't have an example for the action!", new { type, actionName });

        return actionExamples[actionName];
    }
}
