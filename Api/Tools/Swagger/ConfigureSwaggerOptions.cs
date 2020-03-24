using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace GamingCommunityApi.Api.Tools.Swagger
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;
        readonly IWebHostEnvironment _env;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        /// <param name="env"></param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IWebHostEnvironment env)
        {
            _provider = provider;
            _env = env;
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description, _env));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description, IWebHostEnvironment env)
        {
            var description_html = ""
                + "<h5><strong>Gamers</strong>, Do you want a big community just for ourselves? Let's make it together.</h5>"
                + "<h5>This is the place where we can communicate together with our beloved games.</h5><br>"
                + "";

            if (env.IsProduction())
            {
                description_html += "<h5>Sample urls:</h5>"
                    + "<div><a href=\"https://api.gaming-community.bitiano.com/users/{your-id}\">https://api.gaming-community.bitiano.com/users/{your-id}</a></div>"
                    + "<div><a href=\"https://api.gaming-community.bitiano.com/v0.1/users/{your-id}\">https://api.gaming-community.bitiano.com/v0.1/users/{your-id}</a></div>";
            }
            else
            {
                description_html += "<h5>Sample urls:</h5>"
                    + "<div><a href=\"https://localhost:5021/users/{your-id}\">https://localhost:5021/users/{your-id}</a></div>"
                    + "<div><a href=\"https://localhost:5021/v0.1/users/{your-id}\">https://localhost:5021/v0.1/users/{your-id}</a></div>";
            }

            var info = new OpenApiInfo()
            {
                Title = "Gaming Community Api",
                Version = description.ApiVersion.ToString(),
                Description = description_html,
                Contact = new OpenApiContact
                {
                    Name = "Soroush Kavousi",
                    Email = "soroushkavousi.me@gmail.com",
                },
                License = new OpenApiLicense
                {
                    Name = "Use under LICX",
                    Url = new Uri("https://example.com/license")
                },
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }
            
            return info;
        }
    }
}
