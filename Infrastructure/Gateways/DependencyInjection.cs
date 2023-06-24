using FireplaceApi.Application.Emails;
using FireplaceApi.Application.Files;
using FireplaceApi.Application.GoogleUsers;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace FireplaceApi.Infrastructure.Gateways;

public static class DependencyInjection
{
    public static void AddGateways(this IServiceCollection services)
    {
        services.AddSingleton<IEmailGateway, GmailGateway>();
        services.AddScoped<IFileGateway, FileGateway>();
        services.AddScoped<IGoogleGateway, GoogleGateway>();
        services.AddRedis();
    }

    private static void AddRedis(this IServiceCollection services)
    {
        services.AddSingleton<RedisGateway>();
        var redisConnection = ConnectionMultiplexer.Connect(Configs.Current.Api.RedisConnectionString);
        services.AddSingleton<IConnectionMultiplexer>(redisConnection);
    }
}
