﻿using FireplaceApi.Application.Comments;
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
using FireplaceApi.Infrastructure.CacheService;
using FireplaceApi.Infrastructure.Gateways;
using FireplaceApi.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Presentation.Extensions;

public static class ServiceInjectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
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
        return services;
    }

    public static IServiceCollection AddCacheServices(this IServiceCollection services)
    {
        services.AddSingleton<IRequestTraceCacheService, RequestTraceCacheService>();
        return services;
    }

    public static IServiceCollection AddGateways(this IServiceCollection services)
    {
        services.AddSingleton<IEmailGateway, GmailGateway>();
        services.AddScoped<IFileGateway, FileGateway>();
        services.AddScoped<IGoogleGateway, GoogleGateway>();
        services.AddSingleton<RedisGateway>();
        return services;
    }

    public static IServiceCollection AddOperators(this IServiceCollection services)
    {
        services.AddScoped<CommentOperator>();
        services.AddScoped<CommunityOperator>();
        services.AddScoped<CommunityMembershipOperator>();
        services.AddScoped<EmailOperator>();
        services.AddScoped<ErrorOperator>();
        services.AddScoped<FileOperator>();
        services.AddScoped<GoogleUserOperator>();
        services.AddScoped<PostOperator>();
        services.AddScoped<RequestTraceOperator>();
        services.AddScoped<SessionOperator>();
        services.AddScoped<UserOperator>();
        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddScoped<Validators.CommentValidator>();
        services.AddScoped<Validators.CommunityValidator>();
        services.AddScoped<Validators.CommunityMembershipValidator>();
        services.AddScoped<Validators.EmailValidator>();
        services.AddScoped<Validators.ErrorValidator>();
        services.AddScoped<Validators.FileValidator>();
        services.AddScoped<Validators.PostValidator>();
        services.AddScoped<Validators.SessionValidator>();
        services.AddScoped<Validators.UserValidator>();

        services.AddScoped<CommentValidator>();
        services.AddScoped<CommunityValidator>();
        services.AddScoped<CommunityMembershipValidator>();
        services.AddScoped<EmailValidator>();
        services.AddScoped<ErrorValidator>();
        services.AddScoped<FileValidator>();
        services.AddScoped<PostValidator>();
        services.AddScoped<SessionValidator>();
        services.AddScoped<UserValidator>();
        return services;
    }

    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<CommentService>();
        services.AddScoped<CommunityService>();
        services.AddScoped<CommunityMembershipService>();
        services.AddScoped<EmailService>();
        services.AddScoped<ErrorService>();
        services.AddScoped<FileService>();
        services.AddScoped<PostService>();
        services.AddScoped<SessionService>();
        services.AddScoped<UserService>();
        return services;
    }
}
