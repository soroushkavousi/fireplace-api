namespace FireplaceApi.Application.Tools
{
    public static class Constants
    {
        public static string LatestApiVersion { get; } = "v1";
        public static string AuthorizationHeaderKey { get; } = "Authorization";
        public static string ExamplePropertyName { get; } = "Example";
        public static string ActionExamplesPropertyName { get; } = "ActionExamples";
        public static string ApiExceptionErrorDtoClassName { get; } = "ApiExceptionErrorDto";
        public static string ResponseAccessTokenHeaderKey { get; } = "access_token";
        public static string ResponseAccessTokenCookieKey { get; } = "access_token";
        public static string RequestingUserKey { get; } = "User";
        public static string X_FORWARDED_FOR { get; } = "X-Forwarded-For";
        public static string CsrfTokenKey { get; } = "X-CSRF-TOKEN";
        public static string SetCookieHeaderKey { get; } = "Set-Cookie";
        public static string ErrorKey { get; } = "Error";
        public static string GraphQLBaseRoute { get; } = "/graphql";
        public static string ActionNameKey { get; } = "ActionName";
    }
}
