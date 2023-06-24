using FireplaceApi.Application;
using FireplaceApi.Infrastructure;
using FireplaceApi.Presentation;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Middlewares;
using FireplaceApi.Presentation.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using System;

var program = new Program();
program.Start(args);

public partial class Program
{
    void Start(string[] args)
    {
        ProjectInitializer.Start();
        var logger = ProjectInitializer.Logger;
        try
        {
            logger.Trace("Starting api...");
            var builder = CreateBuilder(args);
            var app = CreateApp(builder);
            app.Run();
        }
        catch (Exception ex)
        {
            logger.Error(ex, "Stopped program because of exception!");
            throw;
        }
        finally
        {
            logger.Trace("Flushing logger...");
            LogManager.Shutdown();
        }
    }

    WebApplicationBuilder CreateBuilder(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            Args = args,
            EnvironmentName = EnvironmentVariable.EnvironmentName.Value
        });
        builder.Logging.ClearProviders();
        builder.Logging.AddConsole();
        builder.WebHost.UseNLog();
        ConfigureBuilderServices(builder.Services);
        return builder;
    }

    void ConfigureBuilderServices(IServiceCollection services)
    {
        // Add projects
        services.AddInfrastructure(EnvironmentVariable.ConnectionString.Value);
        services.AddApplication();
        services.AddPresentation();
    }

    WebApplication CreateApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        app.UseRequestTracer();
        app.UseApiForwardHeaders();

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Used for injecting additional CSS and JS files into the swagger UI.
        app.UseStaticFiles();
        app.UseApiVersionRewrite();
        app.UseRouting();
        app.UseApiSwagger();
        app.UseApiExceptionHandler();
        app.UseRequestContentValidator();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgeryTokenGenerator();
        app.MapControllers();
        app.MapGraphQL();
        return app;
    }
}
