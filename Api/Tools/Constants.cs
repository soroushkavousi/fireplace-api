namespace FireplaceApi.Api.Tools
{
    public static class Constants
    {
        public static string EnvironmentNameKey { get; } = "ASPNETCORE_ENVIRONMENT";
        public static string ConnectionStringKey { get; } = "ASPNETCORE_CONNECTION_STRING";
        public static string AuthorizationHeaderKey { get; } = "Authorization";
        public static string ExamplePropertyName { get; } = "Example";
        public static string ActionExamplesPropertyName { get; } = "ActionExamples";
        public static string ApiExceptionErrorDtoClassName { get; } = "ApiExceptionErrorDto";
        public static string ResponseAccessTokenHeaderKey { get; } = "access_token";
        public static string ResponseAccessTokenCookieKey { get; } = "access_token";
        public static string RequestingUserActionArgumentKey { get; } = "requestingUser";
        public static string RequestingUserKey { get; } = "User";
        public static string X_FORWARDED_FOR { get; } = "X-Forwarded-For";
        public static string CsrfTokenKey { get; } = "X-CSRF-TOKEN";
        public static string SetCookieHeaderKey { get; } = "Set-Cookie";
    }
}
