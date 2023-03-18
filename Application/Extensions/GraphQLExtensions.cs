using FireplaceApi.Application.Middlewares;
using FireplaceApi.Application.Resolvers;
using HotChocolate.Execution.Configuration;
using HotChocolate.Resolvers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace FireplaceApi.Application.Extensions
{
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

        public static bool IsResolverAQueryOrMutationExtendedType(this IMiddlewareContext context)
        {
            var typeName = context.ObjectType.Name;
            if (typeName == nameof(GraphQLQuery) || typeName == nameof(GraphQLMutation))
                return true;
            return false;
        }
    }
}
