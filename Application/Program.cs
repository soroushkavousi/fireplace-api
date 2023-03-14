using FireplaceApi.Application.Attributes;
using FireplaceApi.Application.Controllers;
using FireplaceApi.Application.Extensions;
using FireplaceApi.Application.Middlewares;
using FireplaceApi.Application.Tool;
using FireplaceApi.Application.Tools;
using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
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
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

var program = new FireplaceApi.Application.Program();
program.Start(args);


namespace FireplaceApi.Application
{
    public class Program
    {
        private Logger _logger;

        public void Start(string[] args)
        {
            ProjectInitializer.Start();
            _logger = ProjectInitializer.Logger;
            try
            {
                _logger.Trace("Starting api...");
                var builder = CreateBuilder(args);
                var app = CreateApp(builder);
                app.Run();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Stopped program because of exception!");
                throw;
            }
            finally
            {
                _logger.Trace("Flushing logger...");
                LogManager.Shutdown();
            }
        }

        private WebApplicationBuilder CreateBuilder(string[] args)
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

        private void ConfigureBuilderServices(WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            builder.Services.AddSingleton<IObjectModelValidator, NullObjectModelValidator>();
            var infrastructureAssemblyName = $"{nameof(FireplaceApi)}.{nameof(FireplaceApi.Infrastructure)}";
            builder.Services.AddDbContext<FireplaceApiDbContext>(
                optionsBuilder => optionsBuilder.UseNpgsql(
                    EnvironmentVariable.ConnectionString.Value,
                    optionsBuilder => optionsBuilder.MigrationsAssembly(infrastructureAssemblyName))
            );
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();
            builder.Services.AddInfrastructurConverters();
            builder.Services.AddRepositories();
            builder.Services.AddGateways();
            builder.Services.AddTools();
            builder.Services.AddOperators();
            builder.Services.AddValidators();
            builder.Services.AddServices();
            builder.Services.AddApiConverters();

            builder.Services.AddAntiforgery(options =>
            {
                options.Cookie = new CookieBuilder
                {
                    Name = Constants.CsrfTokenKey,
                    MaxAge = new TimeSpan(Configs.Current.Api.CookieMaxAgeInDays, 0, 0, 0),
                    IsEssential = true,
                };
                options.FormFieldName = Constants.CsrfTokenKey;
                options.HeaderName = Constants.CsrfTokenKey;
                options.SuppressXFrameOptionsHeader = true;
            });

            builder.Services.AddRouting(options => options.LowercaseUrls = true);
            builder.Services.AddControllers(options =>
            {
                options.Filters.Add(typeof(RequestingUserInjectorAttribute));
                options.Filters.Add(typeof(InputHeaderParametersInjectorAttribute));
                options.Filters.Add(typeof(InputCookieParametersInjectorAttribute));
                options.Filters.Add(typeof(ActionLoggingAttribute));
                options.Filters.Add(typeof(ActionInputValidatorAttribute));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

            builder.Services.AddRouting(options =>
            {
                options.ConstraintMap.Add(UlongRouteConstraint.Name, typeof(UlongRouteConstraint));
            });

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

            builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            builder.Services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations(false, true);
                options.OperationFilter<SwaggerDefaultValueFilter>();
                options.OperationFilter<SwaggerSecurityFilter>();
                options.OperationFilter<ActionResponseExampleProvider>();
                options.OperationFilter<SwaggerEnumFilter>();
                options.DocumentFilter<CustomModelDocumentFilter<ApiExceptionErrorDto>>();

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

                options.DocumentFilter<OrderDocumentFilter>();
            });

            builder.Services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(3);
                //options.ExcludedHosts.Add("example.com");
            });

            //builder.Services.AddHttpsRedirection(options =>
            //{
            //    options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
            //    options.HttpsPort = 5021;
            //});

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });

            builder.Services.AddHostedService<StatusCheckerService>();
            builder.Services.AddHostedService<ConfigLoaderService>();

            builder.Services
                .AddGraphQLServer()
                .UseGraphQLPipeline()
                .AddGraphQLResolvers()
                .AddHttpRequestInterceptor<RequestingUserGlobalState>();

        }

        private WebApplication CreateApp(WebApplicationBuilder builder)
        {
            var app = builder.Build();

            app.UseRequestTracerMiddleware();

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            // Used for injecting additional CSS and JS files into the swagger UI.
            app.UseStaticFiles();

            app.UseRewriter(new RewriteOptions()
                .AddRewrite(@"^(?!v\d)(?!docs)(?!graphql)(?!swagger)(.*)", $"{Constants.LatestApiVersion}/$1", false));
            app.UseRouting();

            app.UseSwagger(options =>
            {
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                options.DocumentTitle = "Fireplace Api Docs";
                options.EnableDeepLinking();
                options.RoutePrefix = "docs";
                options.SwaggerEndpoint($"/docs/{Constants.LatestApiVersion}/swagger.json", Constants.LatestApiVersion.ToUpper());
                options.DocExpansion(DocExpansion.List);
                options.DisplayRequestDuration();
                options.InjectStylesheet("https://fonts.googleapis.com/css?family=Roboto");
                options.InjectStylesheet("/swagger-ui/custom-swagger-ui.css");
                options.InjectJavascript("https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js");
                options.InjectJavascript("https://apis.google.com/js/platform.js");
                options.InjectJavascript("/swagger-ui/custom-swagger-ui.js");
            });

            app.UseExceptionMiddleware();
            app.UseFirewallMiddleware();
            app.MapControllers();
            app.MapGraphQL();
            return app;
        }
    }
}
