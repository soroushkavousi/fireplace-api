using Microsoft.AspNetCore.Builder;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace FireplaceApi.Presentation.Tools;

public static class SwaggerMiddleware
{
    public static void UseApiSwagger(this IApplicationBuilder app)
    {
        SwaggerBuilderExtensions.UseSwagger(app);
        app.UseSwaggerUI(options =>
        {
            options.DocumentTitle = "Fireplace API Swagger UI";
            options.EnableDeepLinking();
            options.SwaggerEndpoint($"/swagger/{Constants.LatestApiVersion}/swagger.json",
                Constants.LatestApiVersion.ToUpper());
            options.DocExpansion(DocExpansion.List);
            options.DisplayRequestDuration();
            options.InjectStylesheet("https://fonts.googleapis.com/css?family=Roboto");
            options.InjectStylesheet("/swagger-ui/custom-swagger-ui.css");
            options.InjectJavascript("https://ajax.googleapis.com/ajax/libs/jquery/3.6.0/jquery.min.js");
            options.InjectJavascript("https://apis.google.com/js/platform.js");
            options.InjectJavascript("/swagger-ui/custom-swagger-ui.js");
            options.UseRequestInterceptor("(req) => {" +
                    "if (req.method == 'POST' && !('Content-Type' in req.headers))" +
                        "req.headers['Content-Type'] = 'application/json';" +
                    "return req; " +
                "}");
        });
    }
}
