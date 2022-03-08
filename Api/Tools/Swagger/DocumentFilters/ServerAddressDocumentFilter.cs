using FireplaceApi.Core.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace FireplaceApi.Api.Tools
{
    public class ServerAddressDocumentFilter : IDocumentFilter
    {
        private readonly ILogger<ServerAddressDocumentFilter> _logger;
        private readonly string _swaggerDocHost;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ServerAddressDocumentFilter(ILogger<ServerAddressDocumentFilter> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            var scheme = httpContextAccessor?.HttpContext?.Request?.Scheme;
            var host = httpContextAccessor?.HttpContext?.Request?.Host;
            var pathBase = httpContextAccessor?.HttpContext?.Request?.PathBase;
            _swaggerDocHost = $"{scheme}://{host}{pathBase}";
            _httpContextAccessor = httpContextAccessor;
            logger.LogAppInformation($"_swaggerDocHost : {_swaggerDocHost}");
        }

        public void Apply(OpenApiDocument openapiDoc, DocumentFilterContext context)
        {
            openapiDoc.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = _swaggerDocHost } };
        }
    }
}
