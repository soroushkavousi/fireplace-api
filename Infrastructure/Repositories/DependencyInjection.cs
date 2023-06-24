using FireplaceApi.Application.Comments;
using FireplaceApi.Application.Communities;
using FireplaceApi.Application.Configurations;
using FireplaceApi.Application.Emails;
using FireplaceApi.Application.Errors;
using FireplaceApi.Application.Files;
using FireplaceApi.Application.GoogleUsers;
using FireplaceApi.Application.Posts;
using FireplaceApi.Application.RequestTraces;
using FireplaceApi.Application.Sessions;
using FireplaceApi.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Infrastructure.Repositories;

public static class DependencyInjection
{
    public static void AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<ICommentRepository, CommentRepository>();
        services.AddScoped<ICommentVoteRepository, CommentVoteRepository>();
        services.AddScoped<ICommunityRepository, CommunityRepository>();
        services.AddScoped<ICommunityMembershipRepository, CommunityMembershipRepository>();
        services.AddScoped<IConfigsRepository, ConfigsRepository>();
        services.AddScoped<IEmailRepository, EmailRepository>();
        services.AddScoped<IErrorRepository, ErrorRepository>();
        services.AddScoped<IFileRepository, FileRepository>();
        services.AddScoped<IGoogleUserRepository, GoogleUserRepository>();
        services.AddScoped<IPostRepository, PostRepository>();
        services.AddScoped<IPostVoteRepository, PostVoteRepository>();
        services.AddScoped<IRequestTraceRepository, RequestTraceRepository>();
        services.AddScoped<ISessionRepository, SessionRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
    }
}
