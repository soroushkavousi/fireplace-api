using FireplaceApi.Presentation.Tools;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Rewrite;

namespace FireplaceApi.Presentation.Middlewares;

public static class Utilities
{
    public static void UseApiForwardHeaders(this IApplicationBuilder app)
    {
        app.UseForwardedHeaders(new ForwardedHeadersOptions
        {
            ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
        });
    }

    public static void UseApiVersionRewrite(this IApplicationBuilder app)
    {
        app.UseRewriter(new RewriteOptions()
            .AddRewrite(@"^(?!v\d)(?!graphql)(?!swagger)(.*)",
            $"{Constants.LatestApiVersion}/$1", false));
    }
}
