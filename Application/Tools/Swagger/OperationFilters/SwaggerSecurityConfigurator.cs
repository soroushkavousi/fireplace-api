using FireplaceApi.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FireplaceApi.Application.Tools
{
    // Fix security sign, and add csrf token to input headers
    public class SwaggerSecurityConfigurator : IOperationFilter
    {
        private readonly static OpenApiSecurityRequirement _securityRequirement = new()
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Id = "Bearer",
                        //Id = "oauth2",
                        Type = ReferenceType.SecurityScheme,
                    },
                },
                new List<string>()
            }
        };

        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (DoesOperationHasAllowAnonymousAttribute(context))
                return;
            AddSecuritySign(operation, context);
            AddCsrfTokenToInputHeaders(operation, context);
        }

        public void AddSecuritySign(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Security = new List<OpenApiSecurityRequirement> { _securityRequirement };
        }

        public void AddCsrfTokenToInputHeaders(OpenApiOperation operation, OperationFilterContext context)
        {
            if (context.ApiDescription.HttpMethod.IsSafeHttpMethod())
                return;

            if (operation.Parameters == null)
                operation.Parameters = new List<OpenApiParameter>();

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = Constants.CsrfTokenKey,
                In = ParameterLocation.Header,
                Schema = new OpenApiSchema { Type = "string" },
                Required = true,
                Description = "It should be read from the cookie named 'X-CSRF-TOKEN.' However, in swagger UI, it will load automatically."
            });
        }

        private bool DoesOperationHasAllowAnonymousAttribute(OperationFilterContext context)
        {
            context.ApiDescription.TryGetMethodInfo(out var methodInfo);

            if (methodInfo == null)
                return false;

            if (methodInfo.MemberType != MemberTypes.Method)
                return false;

            var hasAllowAnonymousAttribute = methodInfo.GetCustomAttributes(true).OfType<AllowAnonymousAttribute>().Any();
            var actionName = methodInfo.Name;

            if (hasAllowAnonymousAttribute)
                return true;
            return false;
        }
    }
}