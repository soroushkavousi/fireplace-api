using FireplaceApi.Api.Tools;
using Microsoft.AspNetCore.Http;
using System;
using System.Web;

namespace FireplaceApi.Api.Extensions
{
    public static class StringExtensions
    {
        public static bool IsJson(this string strInput)
        {
            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    var obj = System.Text.Json.JsonDocument.Parse(strInput);
                    return true;
                }
                catch (Exception) //some other exception
                {
                    //Console.WriteLine(ex.ToString());
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public static string ToSnakeCase(this string str) =>
            SnakeCaseNamingPolicy.Instance.ConvertName(str);

        public static string ToKebabCase(this string str) =>
            KebabCaseNamingPolicy.Instance.ConvertName(str);

        public static string ToUrlEncoded(this string str) => HttpUtility.UrlEncode(str);

        public static bool IsSafeHttpMethod(this string httpMethod) =>
            HttpMethods.IsGet(httpMethod) ||
            HttpMethods.IsHead(httpMethod) ||
            HttpMethods.IsOptions(httpMethod) ||
            HttpMethods.IsTrace(httpMethod);
    }
}
