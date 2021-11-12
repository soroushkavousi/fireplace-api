using FireplaceApi.Api.Attributes;
using FireplaceApi.Api.Controllers;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Middlewares;
using FireplaceApi.Api.Tools;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json.Serialization;

namespace FireplaceApi.Api
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
            services.AddDbContext<FireplaceApiContext>(
                optionsBuilder => optionsBuilder.UseNpgsql(
                    Configuration.GetConnectionString("MainDatabase"),
                    optionsBuilder => optionsBuilder.MigrationsAssembly("FireplaceApi.Infrastructure"))
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
                options.Filters.Add(typeof(RequestingUserInjectorAttribute));
                options.Filters.Add(typeof(InputHeaderParametersInjectorAttribute));
                options.Filters.Add(typeof(InputCookieParametersInjectorAttribute));
            }).AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = SnakeCaseNamingPolicy.Instance;
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                options.JsonSerializerOptions.Converters.Add(new JsonPropertyOrderConverter());
            });

            services.AddRouting(options =>
            {
                options.ConstraintMap.Add(UlongRouteConstraint.Name, typeof(UlongRouteConstraint));
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

            //var fireplaceApiContext = services.BuildServiceProvider()
            //           .GetService<FireplaceApiContext>();

            //services.AddAuthenticationCore().AddGoogle();

            //services.AddAuthentication("ApiAuthentication")
            //    .AddScheme<AuthenticationSchemeOptions, ApiAuthenticationHandler>("ApiAuthentication", null);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
        {
            app.UseRequestDurationMiddleware();

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
                // build a swagger endpoint for each discovered API version
                foreach (var description in provider.ApiVersionDescriptions.OrderByDescending(x => x.ApiVersion).ToList())
                {
                    options.SwaggerEndpoint($"/docs/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                }
                options.DocExpansion(DocExpansion.List);
                //Console.WriteLine($"System.Reflection.Assembly.GetExecutingAssembly().GetManifestResourceNames();: \n{String.Join("\n", typeof(Startup).Assembly.GetManifestResourceNames())}");
                //options.InjectStylesheet(@"D:\Projects\Fireplace\FireplaceApi\Codes\Api\Tools\Swagger\custom-swagger-ui.css");
                options.DisplayRequestDuration();
                options.InjectStylesheet("https://fonts.googleapis.com/css?family=Roboto");
                options.InjectStylesheet("/swagger-ui/custom-swagger-ui.css");
                options.InjectJavascript("https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js");
                options.InjectJavascript("https://apis.google.com/js/platform.js");
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
