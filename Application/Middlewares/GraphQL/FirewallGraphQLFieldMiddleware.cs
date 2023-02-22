using HotChocolate;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using HotChocolate.Types.Descriptors;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Middlewares.GraphQL
{
    public class FirewallGraphQLFieldMiddleware
    {
        private readonly FieldDelegate _next;

        public FirewallGraphQLFieldMiddleware(FieldDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(IMiddlewareContext context)
        {
            //throw new ApiException(Core.Enums.ErrorName.ACCESS_DENIED, "GraphQL access denied!");
            //throw new GraphQLException("Access Denied!");
            await _next(context);
        }
    }

    public class UseFirewallGraphQLAttribute : ObjectFieldDescriptorAttribute
    {
        public UseFirewallGraphQLAttribute([CallerLineNumber] int order = 0)
        {
            Order = order;
        }

        public override void OnConfigure(IDescriptorContext context,
            IObjectFieldDescriptor descriptor, MemberInfo member)
        {
            descriptor.Use<FirewallGraphQLFieldMiddleware>();
        }
    }

}
