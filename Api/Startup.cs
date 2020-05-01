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
                optionsBuilder => optionsBuilder.UseNpgsql(Configuration.GetConnectionString("MainDatabase"))
            );
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
                // reporting api versions will return the headers "api-supported-versions" and "api-deprecated-versions"
                options.ReportApiVersions = true;
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });

            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                // add a custom operation filter which sets default values
                options.EnableAnnotations(false);
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

                options.AddSecurityDefinition("oauth2", 
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Scheme = "bearer",
                        In = ParameterLocation.Header,
                        Flows = new OpenApiOAuthFlows
                        {
                            AuthorizationCode = new OpenApiOAuthFlow
                            //Implicit = new OpenApiOAuthFlow
                            //ClientCredentials = new OpenApiOAuthFlow
                            //Password = new OpenApiOAuthFlow
                            {
                                AuthorizationUrl = new Uri("https://accounts.google.com/o/oauth2/v2/auth"),
                                TokenUrl = new Uri("https://www.googleapis.com/oauth2/v4/token"),
                                Scopes = new Dictionary<string, string>
                                {
                                    { "profile", "View your basic profile info" },
                                    { "email", "View your email address" },
                                    { "openid", "Authenticate using OpenID Connect" }
                                },
                            },
                        }
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

            var gamingCommunityApiContext = new GamingCommunityApiContext(Configuration.GetConnectionString("MainDatabase"));
            var globalValues = gamingCommunityApiContext.GlobalEntities.AsNoTracking().Where(e => e.Id == GlobalId.RELEASE.To<int>()).Single().Values;
            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.ClientId = globalValues.GoogleClientId;
                    options.ClientSecret = globalValues.GoogleClientSecret;
                    options.AccessType = "offline";
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
                app.UseDatabaseErrorPage();
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
                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                options.DocExpansion(DocExpansion.List);
                options.OAuthClientId("850788234195-6f0f934vgiqj8uet7q418r383llgluba.apps.googleusercontent.com");
                options.OAuthAppName("Gaming Community");
                options.OAuth2RedirectUrl("https://localhost:5021/v0.1/users/sign-up-with-google");
                var relativeApiSwaggerJavascriptPath = @"Codes\Api\Tools\Swagger\nlog.config";
                var solutionDirectory = Core.Tools.Utils.GetSolutionDirectory();
                //var logConfigPath = Path.Combine(solutionDirectory, relativeLogConfigPath);
                //options.InjectJavascript("")
                //options.OAuthAdditionalQueryStringParams(new Dictionary<string, string> { ["response_type"] = "code" });
            });
            app.UseRequestResponseLoggingMiddleware();
            app.UseExceptionMiddleware();
            app.UseHeaderParametersMiddleware();
            app.UseCookieParametersMiddleware();
            //app.UseAuthentication();
            app.UseFirewallMiddleware();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
