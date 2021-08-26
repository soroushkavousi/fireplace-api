using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Tools
{
    public static class Constants
    {
        public static string AuthorizationHeaderKey { get; } = "Authorization";
        public static string ControllerInputHeaderParametersKey { get; } = "ControllerInputHeaderParameters";
        public static string ControllerOutputHeaderParametersKey { get; } = "ControllerOutputHeaderParameters";
        public static string ControllerInputCookieParametersKey { get; } = "ControllerInputCookieParameters";
        public static string ControllerOutputCookieParametersKey { get; } = "ControllerOutputCookieParameters";
        public static string ExamplePropertyName { get; } = "Example";
        public static string ActionExamplesPropertyName { get; } = "ActionExamples";
        public static string FilesBaseUrlPathKey { get; } = "Files:BaseUrlPath";
        public static string FilesBasePhysicalPathKey { get; } = "Files:BasePhysicalPath";
        public static int FileNameLength { get; } = 12;
        public static string ApiExceptionErrorDtoClassName { get; } = "ApiExceptionErrorDto";
        public static string ResponseAccessTokenHeaderKey { get; } = "access_token";
        public static string ResponseAccessTokenCookieKey { get; } = "access_token";
        public static string RequesterUserActionArgumentKey { get; } = "requesterUser";
        public static string InputHeaderParametersActionArgumentKey { get; } = "inputHeaderParameters";
        public static string InputCookieParametersActionArgumentKey { get; } = "inputCookieParameters";
        public static string RequesterUserKey { get; } = "User";
        public static string X_FORWARDED_FOR { get;} = "X-Forwarded-For";
    }
}
