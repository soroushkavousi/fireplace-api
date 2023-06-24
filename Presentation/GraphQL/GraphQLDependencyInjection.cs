using FireplaceApi.Presentation.GraphQL.Resolvers.Queries;
using FireplaceApi.Presentation.Middlewares;
using HotChocolate.Execution.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FireplaceApi.Presentation.GraphQL;

public static class GraphQLDependencyInjection
{
    public static void AddGraphQL(this IServiceCollection services)
    {
        services
            .AddGraphQLServer()
            .UseGraphQLPipeline()
            .AddGraphQLResolvers()
            .AddHttpRequestInterceptor<RequestingUserGlobalState>();
    }

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
}
