using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Presentation.Attributes;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Presentation.Extensions;
using FireplaceApi.Presentation.Middlewares;
using FireplaceApi.Presentation.Tools;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using StackExchange.Redis;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

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
        ConfigureBuilderServices(builder);
        return builder;
    }

    void ConfigureBuilderServices(WebApplicationBuilder builder)
    {
        // Dependency injection items
        builder.Services.AddServices();
        builder.Services.AddValidators();
        builder.Services.AddOperators();
        builder.Services.AddGateways();
        builder.Services.AddRepositories();
        builder.Services.AddCacheServices();
        builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

        // Set database
        var infrastructureAssemblyName = $"{nameof(FireplaceApi)}.{nameof(FireplaceApi.Infrastructure)}";
        builder.Services.AddDbContext<ProjectDbContext>(
            optionsBuilder => optionsBuilder.UseNpgsql(
                EnvironmentVariable.ConnectionString.Value,
                optionsBuilder => optionsBuilder.MigrationsAssembly(infrastructureAssemblyName))
        );

        // Set Redis
        var redisConnection = ConnectionMultiplexer.Connect(Configs.Current.Api.RedisConnectionString);
        builder.Services.AddSingleton<IConnectionMultiplexer>(redisConnection);

        // Config Auth
        builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddApiCookieAuthentication();
        builder.Services.AddAuthorization(options => options.AddApiPolicies());
        builder.Services.AddAuthHandlers();

        // For XSRF/CSRF attacks, add access to IAntiforgery
        builder.Services.AddApiAntiforgery();

        // Make url lowercase
        builder.Services.AddRouting(options => options.LowercaseUrls = true);

        // Config controllers, serializers
        builder.Services.AddControllers(options =>
        {
            options.Filters.Add<RequestingUserInjectorAttribute>();
            options.Filters.Add<InputHeaderDtoInjectorAttribute>();
            options.Filters.Add<InputCookieDtoInjectorAttribute>();
            options.Filters.Add<ActionLoggingAttribute>();
            options.Filters.Add<ActionInputValidatorAttribute>();
        }).AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        // Add ulong support
        builder.Services.AddRouting(options =>
        {
            options.ConstraintMap.Add(UlongRouteConstraint.Name, typeof(UlongRouteConstraint));
        });

        // Add API versioning
        builder.Services.AddApiVersioning(options =>
        {
            options.AssumeDefaultVersionWhenUnspecified = true;
            options.DefaultApiVersion = new ApiVersion(1, 0);
            options.ReportApiVersions = true;
        });

        builder.Services.AddVersionedApiExplorer(options =>
        {
            options.GroupNameFormat = "'v'VVV";
            options.SubstituteApiVersionInUrl = true;
        });

        //Config Swagger
        builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
        builder.Services.AddSwaggerGen(options =>
        {
            options.EnableAnnotations(false, true);
            options.OperationFilter<SwaggerDefaultValueProvider>();
            options.OperationFilter<ActionResponseExampleProvider>();
            options.OperationFilter<SwaggerEnumDescriptionProvider>();
            options.DocumentFilter<CustomModelDocumentFilter<ApiExceptionErrorDto>>();
            options.DocumentFilter<OrderDocumentFilter>();

            // Enable xml
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            options.IncludeXmlComments(xmlPath);

            options.AddSecurityDefinition("Bearer",
                new OpenApiSecurityScheme
                {
                    Description = "Authorization header using the Bearer scheme." +
                        " <br/> <br/> Just put your **access token** as the value. <br/>",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    In = ParameterLocation.Header,
                });
            options.OperationFilter<SwaggerSecurityConfigurator>();
        });

        // Suppress default validation
        builder.Services.AddSingleton<IObjectModelValidator, NullObjectModelValidator>();
        builder.Services.Configure<ApiBehaviorOptions>(options =>
        {
            options.SuppressModelStateInvalidFilter = true;
            options.SuppressMapClientErrors = true;
        });

        // Add a hosted service to check the API readiness
        builder.Services.AddHostedService<ReadinessCheckerService>();

        // Add GraphQL
        builder.Services
            .AddGraphQLServer()
            .UseGraphQLPipeline()
            .AddGraphQLResolvers()
            .AddHttpRequestInterceptor<RequestingUserGlobalState>();
    }

    WebApplication CreateApp(WebApplicationBuilder builder)
    {
        var app = builder.Build();

        app.UseRequestTracer();

        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });

        if (app.Environment.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Used for injecting additional CSS and JS files into the swagger UI.
        app.UseStaticFiles();

        app.UseRewriter(new RewriteOptions()
            .AddRewrite(@"^(?!v\d)(?!graphql)(?!swagger)(.*)",
            $"{Constants.LatestApiVersion}/$1", false));
        app.UseRouting();

        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "Fireplace API Swagger UI";
            options.EnableDeepLinking();
            options.SwaggerEndpoint($"/swagger/{Constants.LatestApiVersion}/swagger.json",
                Constants.LatestApiVersion.ToUpper());
            options.DocExpansion(DocExpansion.List);
            options.DisplayRequestDuration();
            options.InjectStylesheet("https://fonts.googleapis.com/css?family=Roboto");
            options.InjectStylesheet("/swagger-ui/custom-swagger-ui.css");
            options.InjectJavascript("https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js");
            options.InjectJavascript("https://apis.google.com/js/platform.js");
            options.InjectJavascript("/swagger-ui/custom-swagger-ui.js");
            options.UseRequestInterceptor("(req) => {" +
                    "if (req.method == 'POST' && !('Content-Type' in req.headers))" +
                        "req.headers['Content-Type'] = 'application/json';" +
                    "return req; " +
                "}");
        });

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
