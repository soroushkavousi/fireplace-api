using FireplaceApi.Application.Resolvers;
using FireplaceApi.Domain.Extensions;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Middlewares
{
    public class ResolverLoggingFieldMiddleware
    {
        private readonly FieldDelegate _next;
        private readonly ILogger<ResolverLoggingFieldMiddleware> _logger;

        public ResolverLoggingFieldMiddleware(FieldDelegate next,
            ILogger<ResolverLoggingFieldMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            var name = context.ObjectType.Name;
            if (name != nameof(GraphQLQuery) && name != nameof(GraphQLMutation))
            {
                await _next(context);
                return;
            }
            var sw = Stopwatch.StartNew();
            var arguments = (IReadOnlyDictionary<string, ArgumentValue>)context.GetType().GetProperty("Arguments").GetValue(context, null);
            var argumentKeys = arguments.Keys;
            var inputs = new Dictionary<string, object>();
            foreach (var key in argumentKeys)
            {
                var value = context.ArgumentValue<object>(key);
                inputs.Add(key, value);
            }
            var field = (ObjectField)context.GetType().GetProperty("Field").GetValue(context, null);
            var fieldName = field.Name.ToUpper();
            _logger.LogAppInformation(message: $"resolver: {fieldName}", title: "RESOLVER_INPUT", parameters: inputs);
            await _next(context);
            _logger.LogAppInformation(sw: sw, message: $"resolver: {fieldName}", title: "RESOLVER_OUTPUT", parameters: context.Result);
        }
    }
}
