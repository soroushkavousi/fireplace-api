using HotChocolate.Resolvers;
using HotChocolate.Types;

namespace FireplaceApi.Presentation.Tools;

public static class GraphQLExtensions
{
    public static ObjectField GetField(this IMiddlewareContext context)
        => (ObjectField)context.GetType().GetProperty("Field").GetValue(context, null);

    public static bool IsApiResolver(this IMiddlewareContext context)
    {
        var field = context.GetField();
        if (field.ResolverMember.MemberType == System.Reflection.MemberTypes.Method)
            return true;
        return false;
    }
}
