using FireplaceApi.Api.Extensions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Operators;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Tools
{
    public class TypeExampleProvider : ISchemaFilter
    {
        private readonly ILogger<TypeExampleProvider> _logger;
        private readonly IServiceProvider _serviceProvider;

        public TypeExampleProvider(ILogger<TypeExampleProvider> logger, IServiceProvider serviceProvider)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var example = context.Type.ExtractExample();

            if (context.Type.Name.Contains("Error") && example.ContainsKey("message") == false)
                example = FillErrorExampleFromName(example, _serviceProvider, _logger).GetAwaiter().GetResult();

            schema.Example = example;
            schema.Default = example;
        }

        public static async Task<OpenApiObject> FillErrorExampleFromName<T>(OpenApiObject errorExample,
            IServiceProvider serviceProvider, ILogger<T> logger)
        {
            var name = errorExample["name"].To<OpenApiString>().Value.ToEnum<ErrorName>();
            Error error = null;
            using (var scope = serviceProvider.CreateScope())
            {
                try
                {
                    var errorOperator = scope.ServiceProvider.GetService<ErrorOperator>();
                    error = await errorOperator.GetErrorByNameAsync(name);
                }
                catch (Exception ex)
                {
                    logger.LogAppInformation($"Exception occurred when trying to get error {name} details for examples => {ex.Message}");
                }
                finally
                {
                    if (error == null)
                    {
                        logger.LogAppInformation($"Error {name} seems does not exists yet or an exception occurred.");
                        error = Error.InternalServerError;
                    }
                }
            }
            errorExample.Remove("name");
            errorExample["code"] = new OpenApiInteger(error.Code);
            errorExample["message"] = new OpenApiString(error.ClientMessage);
            return errorExample;
        }
    }
}
