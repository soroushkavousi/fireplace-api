using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GamingCommunityApi.Tools.Swagger.DocumentFilters
{
    //Usage : "options.DocumentFilter<CustomModelDocumentFilter<Error>>();"
    public class CustomModelDocumentFilter<T> : IDocumentFilter where T : class
    {
        private readonly ILogger<CustomModelDocumentFilter<T>> _logger;
        private readonly string _swaggerDocHost;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CustomModelDocumentFilter(ILogger<CustomModelDocumentFilter<T>> logger, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            var scheme = httpContextAccessor?.HttpContext?.Request?.Scheme;
            var host = httpContextAccessor?.HttpContext?.Request?.Host;
            var pathBase = httpContextAccessor?.HttpContext?.Request?.PathBase;
            _swaggerDocHost = $"{scheme}://{host}{pathBase}";
            _httpContextAccessor = httpContextAccessor;
            logger.LogInformation($"_swaggerDocHost : {_swaggerDocHost}");
        }

        public void Apply(OpenApiDocument openapiDoc, DocumentFilterContext context)
        {
            openapiDoc.Servers = new List<OpenApiServer>() { new OpenApiServer() { Url = _swaggerDocHost } };
            context.SchemaGenerator.GenerateSchema(typeof(T), context.SchemaRepository);
        }
    }
}
