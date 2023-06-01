using FireplaceApi.Application.Middlewares;
using FireplaceApi.Application.Resolvers;
using HotChocolate.Execution.Configuration;
using HotChocolate.Resolvers;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FireplaceApi.Application.Extensions;

public static class GraphQLExtensions
{
    public static IRequestExecutorBuilder UseGraphQLPipeline(
        this IRequestExecutorBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        return builder
            .UseDefaultPipeline()
            .UseSampleGraphQLRequestMiddleware()
            .UseField<ResolverLoggingFieldMiddleware>()
            .UseField<ApiExceptionFieldMiddleware>()
            .UseField<FirewallFieldMiddleware>();
    }

    public static IRequestExecutorBuilder AddGraphQLResolvers(
        this IRequestExecutorBuilder builder)
    {
        if (builder is null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        builder = builder
            .AddQueryType<GraphQLQuery>()
            .AddTypeExtension<UserQueryResolvers>()
            .AddTypeExtension<CommunityQueryResolvers>()
            .AddTypeExtension<PostCommunityQueryResolvers>()
            .AddTypeExtension<UserCommunitiesQueryResolvers>()
            .AddTypeExtension<PostQueryResolvers>()
            .AddTypeExtension<CommunityPostsQueryResolvers>()
            .AddTypeExtension<CommentPostQueryResolvers>()
            .AddTypeExtension<UserPostsQueryResolvers>()
            .AddTypeExtension<CommentQueryResolvers>()
            .AddTypeExtension<PostCommentsQueryResolvers>()
            .AddTypeExtension<UserCommentsQueryResolvers>();

        builder = builder
            .AddMutationType<GraphQLMutation>()
            .AddTypeExtension<CommunityMutationResolvers>();

        return builder;
    }

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
