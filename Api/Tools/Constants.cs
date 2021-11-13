namespace FireplaceApi.Api.Tools
{
    public static class Constants
    {
        public static string AuthorizationHeaderKey { get; } = "Authorization";
        public static string InputHeaderParametersKey { get; } = "InputHeaderParameters";
        public static string OutputHeaderParametersKey { get; } = "OutputHeaderParameters";
        public static string InputCookieParametersKey { get; } = "InputCookieParameters";
        public static string OutputCookieParametersKey { get; } = "OutputCookieParameters";
        public static string ExamplePropertyName { get; } = "Example";
        public static string ActionExamplesPropertyName { get; } = "ActionExamples";
        public static string FilesBaseUrlPathKey { get; } = "Files:BaseUrlPath";
        public static string FilesBasePhysicalPathKey { get; } = "Files:BasePhysicalPath";
        public static int FileNameLength { get; } = 12;
        public static string ApiExceptionErrorDtoClassName { get; } = "ApiExceptionErrorDto";
        public static string ResponseAccessTokenHeaderKey { get; } = "access_token";
        public static string ResponseAccessTokenCookieKey { get; } = "access_token";
        public static string RequestingUserActionArgumentKey { get; } = "requestingUser";
        public static string InputHeaderParametersActionArgumentKey { get; } = "inputHeaderParameters";
        public static string InputCookieParametersActionArgumentKey { get; } = "inputCookieParameters";
        public static string RequestingUserKey { get; } = "User";
        public static string X_FORWARDED_FOR { get; } = "X-Forwarded-For";
    }
}
