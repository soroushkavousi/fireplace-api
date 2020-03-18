using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;

namespace GamingCommunityApi.Tools.Swagger
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
            var description_html = "A simple ASP.NET Core Web Api currently under version 0.2.";
            if (env.IsProduction())
            {
                description_html += "<h5>Sample urls:</h5>" +
                "<div><a href=\"https://api.gaming-community.bitiano.com/users\">https://api.gaming-community.bitiano.com/users</a></div>" +
                "<div><a href=\"https://api.gaming-community.bitiano.com/v0.2/users\">https://api.gaming-community.bitiano.com/v0.2/users</a></div>" +
                "<div><a href=\"https://api.gaming-community.bitiano.com/v0.1/users\">https://api.gaming-community.bitiano.com/v0.1/users</a></div>";
            }
            else
            {
                description_html += "<h5>Sample urls:</h5>" +
                "<div><a href=\"https://localhost:5011/users\">https://localhost:5011/users</a></div>" +
                "<div><a href=\"https://localhost:5011/v0.2/users\">https://localhost:5011/v0.2/users</a></div>" +
                "<div><a href=\"https://localhost:5011/v0.1/users\">https://localhost:5011/v0.1/users</a></div>";
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
