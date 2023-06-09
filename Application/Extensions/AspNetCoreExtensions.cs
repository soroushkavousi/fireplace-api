using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Services;
using FireplaceApi.Domain.Tools;
using FireplaceApi.Infrastructure.CacheService;
using FireplaceApi.Infrastructure.Gateways;
using FireplaceApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FireplaceApi.Application.Extensions;

public static class AspNetCoreExtensions
{
    public static IEnumerable<string> GetErrorMessages(this ModelStateDictionary modelState)
    {
        return modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
    }

    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
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

    public static IServiceCollection AddTools(this IServiceCollection services)
    {
        services.AddScoped<Firewall>();
        return services;
    }

    public static IServiceCollection AddOperators(this IServiceCollection services)
    {
        services.AddScoped<AccessTokenOperator>();
        services.AddScoped<CommentOperator>();
        services.AddScoped<CommunityOperator>();
        services.AddScoped<CommunityMembershipOperator>();
        services.AddScoped<ConfigsOperator>();
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

        services.AddScoped<Domain.Validators.AccessTokenValidator>();
        services.AddScoped<Domain.Validators.CommentValidator>();
        services.AddScoped<Domain.Validators.CommunityValidator>();
        services.AddScoped<Domain.Validators.CommunityMembershipValidator>();
        services.AddScoped<Domain.Validators.EmailValidator>();
        services.AddScoped<Domain.Validators.ErrorValidator>();
        services.AddScoped<Domain.Validators.FileValidator>();
        services.AddScoped<Domain.Validators.PostValidator>();
        services.AddScoped<Domain.Validators.SessionValidator>();
        services.AddScoped<Domain.Validators.UserValidator>();
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

    public static Expression<TDelegate> AndAlso<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right)
    {
        return Expression.Lambda<TDelegate>(Expression.AndAlso(left, right), left.Parameters);
    }
}
