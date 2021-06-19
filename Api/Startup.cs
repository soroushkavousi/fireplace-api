using System;
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Diagnostics.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using GamingCommunityApi.Api.Tools.Swagger;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Rewrite;
using System.Net;
using GamingCommunityApi.Api.Middlewares;
using GamingCommunityApi.Api.Extensions;
using System.Text.Json;
using System.Diagnostics;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.Json.Serialization;
using GamingCommunityApi.Api.Tools.AspDotNetCore;
using Swashbuckle.AspNetCore.SwaggerUI;
using GamingCommunityApi.Api.Controllers.Parameters.ErrorParameters;
using GamingCommunityApi.Api.Tools.Swagger.OperationFilters;
using GamingCommunityApi.Api.Tools.Swagger.DocumentFilters;
using GamingCommunityApi.Api.Attributes.ActionFilterAttributes;
using Microsoft.AspNetCore.Authentication;
using GamingCommunityApi.Infrastructure.Entities;
using GamingCommunityApi.Api.Tools.TextJsonSerializer;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.Extensions;

namespace GamingCommunityApi.Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IObjectModelValidator, NullObjectModelValidator>();
            services.AddDbContext<GamingCommunityApiContext>(
                optionsBuilder => optionsBuilder.UseNpgsql(
                    Configuration.GetConnectionString("MainDatabase"),
                    optionsBuilder => optionsBuilder.MigrationsAssembly("GamingCommunityApi.Infrastructure"))
            );
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddInfrastructurConverters();
            services.AddRepositories();
            services.AddGateways();
            services.AddTools();
            services.AddOperators();
            services.AddValidators();
            services.AddServices();
            services.AddApiConverters();

            services.AddRouting(options => options.LowercaseUrls = true);
            services.AddControllers(options =>
            {
                options.Filters.Add(typeof(RequesterUserInjectorAttribute));
                options.Filters.Add(typeof(InputHeaderParametersInjectorAttribute));
                options.Filters.Add(typeof(InputCookieParametersInjectorAttribute));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
            //services.AddControllers().AddNewtonsoftJson(options =>
            //{
            //    options.SerializerSettings.ContractResolver = new DefaultContractResolver()
            //    {
            //        NamingStrategy = new SnakeCaseNamingStrategy()
            //    };
            //});

            services.AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations(false, true);
                options.OperationFilter<SwaggerDefaultValues>();
                options.OperationFilter<ActionResponseExampleProvider>();
                options.DocumentFilter<CustomModelDocumentFilter<ApiExceptionErrorDto>>();

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "Authorization header using the Bearer scheme.",
                        Type = SecuritySchemeType.Http,
                        Scheme = "bearer",
                        In = ParameterLocation.Header,
                    });

                //options.DocInclusionPredicate((docName, apiDesc) =>
                //{
                //    if (!apiDesc.TryGetMethodInfo(out MethodInfo methodInfo)) return false;

                //    var versions = methodInfo.DeclaringType
                //        .GetCustomAttributes(true)
                //        .OfType<ApiVersionAttribute>()
                //        .SelectMany(attr => attr.Versions);

                //    return versions.Any(v => $"v{v.ToString()}" == docName);
                //});

                //options.DescribeAllEnumsAsStrings();
            });

            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(3);
                //options.ExcludedHosts.Add("example.com");
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5021;
            });
            
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
                options.SuppressMapClientErrors = true;
            });

            //var gamingCommunityApiContext = services.BuildServiceProvider()
            //           .GetService<GamingCommunityApiContext>();

            //services.AddAuthenticationCore().AddGoogle();

            //services.AddAuthentication("ApiAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions, ApiAuthenticationHandler>("ApiAuthentication", null);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });


            if (env.IsDevelopment())
            {
                app.UseHsts();
            }
            else
            {
                app.UseDeveloperExceptionPage();
                app.UseMigrationsEndPoint();
            }

            app.UseHttpsRedirection();
            app.UseRewriter(new RewriteOptions()
                // Saving for time that api wants redirecting.
                //.AddRedirect(@"api/(?!v\d)(.*)", "api/v0.2/$1", (int)HttpStatusCode.Redirect)); 
                .AddRewrite(@"^(?!v\d\.)(?!docs)(?!swagger)(.*)", "v0.1/$1", false));
            app.UseRouting();
            //app.UseAuthorization();

            app.UseStaticFiles();

            app.UseSwagger(options =>
            {
                options.PreSerializeFilters.Add((swaggerDoc, httpRequest) =>
                {
                    // hide all SwaggerDocument PathItems with added Security information for OAuth2 without accepted roles (for example, "AcceptedRole")
                    var x = swaggerDoc;
                    var y = httpRequest;
                });
                options.RouteTemplate = "docs/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(options =>
            {
                options.EnableDeepLinking();
                //options.EnableFilter();
                options.RoutePrefix = "docs";
                options.DisplayRequestDuration();
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion).ToList())
                {
                    options.SwaggerEndpoint($"/docs/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                options.DocExpansion(DocExpansion.List);
                //Console.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();: \n{String.Join("\n", typeof(Startup).Assembly.GetManifestResourceNames())}");
                //options.InjectStylesheet(@"D:\Projects\GamingCommunity\GamingCommunityApi\Codes\Api\Tools\Swagger\custom-swagger-ui.css");
                options.DisplayRequestDuration();
                options.InjectStylesheet("/swagger-ui/custom-swagger-ui.css");
                options.InjectJavascript("/swagger-ui/custom-swagger-ui.js");
            });
            app.UseRequestResponseLoggingMiddleware();
            app.UseExceptionMiddleware();
            app.UseHeaderParametersMiddleware();
            app.UseCookieParametersMiddleware();
            app.UseFirewallMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
