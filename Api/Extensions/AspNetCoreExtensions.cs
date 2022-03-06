using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Operators;
using FireplaceApi.Core.Services;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Validators;
using FireplaceApi.Infrastructure.Gateways;
using FireplaceApi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace FireplaceApi.Api.Extensions
{
    public static class AspNetCoreExtensions
    {
        public static IEnumerable<string> GetErrorMessages(this ModelStateDictionary modelState)
        {
            return modelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
        }

        public static IServiceCollection AddInfrastructurConverters(this IServiceCollection services)
        {
            services.AddScoped<Infrastructure.Converters.AccessTokenConverter>();
            services.AddScoped<Infrastructure.Converters.CommentConverter>();
            services.AddScoped<Infrastructure.Converters.CommentVoteConverter>();
            services.AddScoped<Infrastructure.Converters.CommunityConverter>();
            services.AddScoped<Infrastructure.Converters.CommunityMembershipConverter>();
            services.AddScoped<Infrastructure.Converters.EmailConverter>();
            services.AddScoped<Infrastructure.Converters.ErrorConverter>();
            services.AddScoped<Infrastructure.Converters.FileConverter>();
            services.AddScoped<Infrastructure.Converters.GlobalConverter>();
            services.AddScoped<Infrastructure.Converters.GoogleUserConverter>();
            services.AddScoped<Infrastructure.Converters.QueryResultConverter>();
            services.AddScoped<Infrastructure.Converters.PostConverter>();
            services.AddScoped<Infrastructure.Converters.PostVoteConverter>();
            services.AddScoped<Infrastructure.Converters.SessionConverter>();
            services.AddScoped<Infrastructure.Converters.UserConverter>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<ICommentVoteRepository, CommentVoteRepository>();
            services.AddScoped<ICommunityRepository, CommunityRepository>();
            services.AddScoped<ICommunityMembershipRepository, CommunityMembershipRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IErrorRepository, ErrorRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IGlobalRepository, GlobalRepository>();
            services.AddScoped<IGoogleUserRepository, GoogleUserRepository>();
            services.AddScoped<IPostRepository, PostRepository>();
            services.AddScoped<IPostVoteRepository, PostVoteRepository>();
            services.AddScoped<IQueryResultRepository, QueryResultRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection AddGateways(this IServiceCollection services)
        {
            services.AddScoped<IEmailGateway, EmailGateway>();
            services.AddScoped<IFileGateway, FileGateway>();
            services.AddScoped<IGoogleGateway, GoogleGateway>();
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
            services.AddScoped<EmailOperator>();
            services.AddScoped<ErrorOperator>();
            services.AddScoped<FileOperator>();
            services.AddScoped<GlobalOperator>();
            services.AddScoped<GoogleUserOperator>();
            services.AddScoped<PageOperator>();
            services.AddScoped<PostOperator>();
            services.AddScoped<QueryResultOperator>();
            services.AddScoped<SessionOperator>();
            services.AddScoped<UserOperator>();
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<AccessTokenValidator>();
            services.AddScoped<CommentValidator>();
            services.AddScoped<CommunityValidator>();
            services.AddScoped<CommunityMembershipValidator>();
            services.AddScoped<EmailValidator>();
            services.AddScoped<ErrorValidator>();
            services.AddScoped<FileValidator>();
            services.AddScoped<PostValidator>();
            services.AddScoped<QueryResultValidator>();
            services.AddScoped<SessionValidator>();
            services.AddScoped<UserValidator>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<AccessTokenService>();
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

        public static IServiceCollection AddApiConverters(this IServiceCollection services)
        {
            services.AddScoped<CommentConverter>();
            services.AddScoped<CommunityConverter>();
            services.AddScoped<CommunityMembershipConverter>();
            services.AddScoped<EmailConverter>();
            services.AddScoped<ErrorConverter>();
            services.AddScoped<FileConverter>();
            services.AddScoped<GlobalConverter>();
            services.AddScoped<PostConverter>();
            services.AddScoped<SessionConverter>();
            services.AddScoped<UserConverter>();
            return services;
        }

        public static Expression<TDelegate> AndAlso<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right)
        {
            return Expression.Lambda<TDelegate>(Expression.AndAlso(left, right), left.Parameters);
        }
    }
}
