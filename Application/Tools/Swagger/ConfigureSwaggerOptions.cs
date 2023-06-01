using FireplaceApi.Domain.Models;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace FireplaceApi.Application.Tools
{
    /// <summary>
    /// Configures the Swagger generation options.
    /// </summary>
    /// <remarks>This allows API versioning to define a Swagger document per API version after the
    /// <see cref="IApiVersionDescriptionProvider"/> service has been resolved from the service container.</remarks>
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        readonly IApiVersionDescriptionProvider _provider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigureSwaggerOptions"/> class.
        /// </summary>
        /// <param name="provider">The <see cref="IApiVersionDescriptionProvider">provider</see> used to generate Swagger documents.</param>
        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            _provider = provider;
        }

        /// <inheritdoc />
        public void Configure(SwaggerGenOptions options)
        {
            // add a swagger document for each discovered API version
            // note: you might choose to skip or document deprecated API versions differently
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
            }
        }

        static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var description_html = "<div>";

            description_html += $@"
                <div>
                    <p>Fireplace API is <strong>a fully-featured ASP.NET Core Web API sample</strong>, and also <strong>a Reddit API clone</strong> that has communities, posts, and nested comments.</p>
                    <p>Check <a target=""_blank"" href=""https://github.com/soroushkavousi/fireplace-api""><b><i>The GitHub Repository</i></b></a></p>
                    <p>Check <a target=""_blank"" href=""{Configs.Current.Api.BaseUrlPath}/graphql""><b><i>The GraphQL Playground</i></b></a></p>
                </div>";

            description_html += $@"
                <div id=""sample-urls"">
                    <div>Sample urls:</div>
                    <ul>
                        <li><a href=""{Configs.Current.Api.BaseUrlPath}/communities?search=developers"">{Configs.Current.Api.BaseUrlPath}/communities?search=developers</a></li>
                        <li><a href=""{Configs.Current.Api.BaseUrlPath}/{Constants.LatestApiVersion}/communities?search=developers"">{Configs.Current.Api.BaseUrlPath}/{Constants.LatestApiVersion}/communities?search=developers</a></li>
                        <li><a href=""{Configs.Current.Api.BaseUrlPath}/users/me"">{Configs.Current.Api.BaseUrlPath}/users/me</a></li>
                    </ul>
                </div>";

            description_html += $@"
                <div>
                    <p><strong><i>Note:</i></strong> On this page, your access token will be automatically stored in cookies after logging in, and you do not need to use the Authorize button for authentication.</p>
                </div>";

            if (Configs.Current == Configs.Default)
            {
                description_html += @"<br /> <div id=""configs-error"">
                    Error: Could not load configs from the database!!!
                </div> <br />";
            }

            description_html += $@"
                <a id=""google-btn"" target=""_blank"" href=""{Configs.Current.Api.BaseUrlPath}/users/open-google-log-in-page"">
                    <div id=""google-icon-wrapper"">
                        <img id=""google-icon"" src=""https://upload.wikimedia.org/wikipedia/commons/5/53/Google_%22G%22_Logo.svg""/>
                    </div>
                    <p id=""btn-text""><b>Log in with Google</b></p>
                </a>";

            description_html += "</div>";

            var info = new OpenApiInfo()
            {
                Title = "Welcome to Fireplace API",
                Version = description.ApiVersion.ToString(),
                Description = description_html,
                Contact = new OpenApiContact
                {
                    Name = "Soroush Kavousi",
                    Email = "soroushkavousi.me@gmail.com",
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
