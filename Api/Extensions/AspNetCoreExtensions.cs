﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Api.Converters;
using GamingCommunityApi.Core.Services;
using GamingCommunityApi.Core.Operators;
using GamingCommunityApi.Infrastructure.Repositories;
using GamingCommunityApi.Core.Interfaces.IRepositories;
using GamingCommunityApi.Core.Validators;
using GamingCommunityApi.Infrastructure.Gateways;
using GamingCommunityApi.Core.Interfaces.IGateways;
using GamingCommunityApi.Core.Tools;

namespace GamingCommunityApi.Api.Extensions
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
            services.AddScoped<Infrastructure.Converters.EmailConverter>();
            services.AddScoped<Infrastructure.Converters.ErrorConverter>();
            services.AddScoped<Infrastructure.Converters.FileConverter>();
            services.AddScoped<Infrastructure.Converters.GlobalConverter>();
            services.AddScoped<Infrastructure.Converters.SessionConverter>();
            services.AddScoped<Infrastructure.Converters.UserConverter>();
            return services;
        }

        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAccessTokenRepository, AccessTokenRepository>();
            services.AddScoped<IEmailRepository, EmailRepository>();
            services.AddScoped<IErrorRepository, ErrorRepository>();
            services.AddScoped<IFileRepository, FileRepository>();
            services.AddScoped<IGlobalRepository, GlobalRepository>();
            services.AddScoped<ISessionRepository, SessionRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }

        public static IServiceCollection AddGateways(this IServiceCollection services)
        {
            services.AddScoped<IEmailGateway, EmailGateway>();
            services.AddScoped<IFileGateway, FileGateway>();
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
            services.AddScoped<EmailOperator>();
            services.AddScoped<ErrorOperator>();
            services.AddScoped<FileOperator>();
            services.AddScoped<SessionOperator>();
            services.AddScoped<UserOperator>();
            return services;
        }

        public static IServiceCollection AddValidators(this IServiceCollection services)
        {
            services.AddScoped<AccessTokenValidator>();
            services.AddScoped<EmailValidator>();
            services.AddScoped<ErrorValidator>();
            services.AddScoped<FileValidator>();
            services.AddScoped<SessionValidator>();
            services.AddScoped<UserValidator>();
            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services.AddScoped<AccessTokenService>();
            services.AddScoped<EmailService>();
            services.AddScoped<ErrorService>();
            services.AddScoped<FileService>();
            services.AddScoped<SessionService>();
            services.AddScoped<UserService>();
            return services;
        }

        public static IServiceCollection AddApiConverters(this IServiceCollection services)
        {
            services.AddScoped<AccessTokenConverter>();
            services.AddScoped<EmailConverter>();
            services.AddScoped<ErrorConverter>();
            services.AddScoped<FileConverter>();
            services.AddScoped<GlobalConverter>();
            services.AddScoped<SessionConverter>();
            services.AddScoped<UserConverter>();
            return services;
        }

        

        

        //public static Session ExtractSession(this HttpContext context)
        //{
        //    var ipAddress = context.Connection.RemoteIpAddress;
        //    var session = new Session
        //    {
        //        IpAddress = ipAddress
        //    };
        //    return session;
        //}

        //public static string ExtractAccessTokenValue(this IHeaderDictionary headerDictionary)
        //{
        //    string accessTokenValue = null;

        //    var authorizationHeaderValue = headerDictionary.GetValue(Constants.AuthorizationHeaderKey, string.Empty).To<string>();
        //    var match = Regexes.AuthorizationHeader.Match(authorizationHeaderValue);
        //    if(match.Success)
        //    {
        //        accessTokenValue = match.Groups[1].Value;
        //    }
        //    return accessTokenValue;
        //}

        public static Expression<TDelegate> AndAlso<TDelegate>(this Expression<TDelegate> left, Expression<TDelegate> right)
        {
            return Expression.Lambda<TDelegate>(Expression.AndAlso(left, right), left.Parameters);
        }
    }
}