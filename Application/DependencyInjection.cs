using FireplaceApi.Application.Comments;
using FireplaceApi.Application.Emails;
using FireplaceApi.Application.Files;
using FireplaceApi.Application.GoogleUsers;
using FireplaceApi.Application.Posts;
using FireplaceApi.Application.RequestTraces;
using FireplaceApi.Application.Sessions;
using FireplaceApi.Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace FireplaceApi.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        services.AddMediatR();
        services.AddServices();
        services.AddValidators();
        services.AddOperators();
    }

    private static void AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly);
        });
    }

    private static void AddServices(this IServiceCollection services)
    {
        services.AddScoped<CommentService>();
        services.AddScoped<EmailService>();
        services.AddScoped<ErrorService>();
        services.AddScoped<FileService>();
        services.AddScoped<PostService>();
        services.AddScoped<SessionService>();
        services.AddScoped<UserService>();
    }

    private static void AddValidators(this IServiceCollection services)
    {
        services.AddScoped<CommentValidator>();
        services.AddScoped<EmailValidator>();
        services.AddScoped<ErrorValidator>();
        services.AddScoped<FileValidator>();
        services.AddScoped<PostValidator>();
        services.AddScoped<SessionValidator>();
        services.AddScoped<UserValidator>();
    }

    private static void AddOperators(this IServiceCollection services)
    {
        services.AddScoped<CommentOperator>();
        services.AddScoped<EmailOperator>();
        services.AddScoped<ErrorOperator>();
        services.AddScoped<FileOperator>();
        services.AddScoped<GoogleUserOperator>();
        services.AddScoped<PostOperator>();
        services.AddScoped<RequestTraceOperator>();
        services.AddScoped<SessionOperator>();
        services.AddScoped<UserOperator>();
    }
}
