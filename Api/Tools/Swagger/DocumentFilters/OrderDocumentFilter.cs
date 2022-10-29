using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;

namespace FireplaceApi.Api.Tools
{
    public class OrderDocumentFilter : IDocumentFilter
    {
        private readonly ILogger<OrderDocumentFilter> _logger;

        public OrderDocumentFilter(ILogger<OrderDocumentFilter> logger)
        {
            _logger = logger;
        }

        public void Apply(OpenApiDocument swaggerDoc, DocumentFilterContext context)
        {
            swaggerDoc.Tags = new List<OpenApiTag> {
                new OpenApiTag { Name = "Community", },
                new OpenApiTag { Name = "Post", },
                new OpenApiTag { Name = "Comment", },
                new OpenApiTag { Name = "CommunityMembership", },
                new OpenApiTag { Name = "User", },
                new OpenApiTag { Name = "Email", },
                new OpenApiTag { Name = "Session", },
                new OpenApiTag { Name = "File", },
                new OpenApiTag { Name = "Error", },
            };
        }
    }
}
