using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Tools;
using System;

namespace FireplaceApi.Core.Validators
{
    public class ApiValidator
    {
        public void ValidateParameterIsNotMissing(string parameter, string parameterName, ErrorName errorId)
        {
            if (string.IsNullOrWhiteSpace(parameter))
            {
                var serverMessage = $"String parameter ({parameterName}) with input value ({parameter}) is missing!";
                throw new ApiException(errorId, serverMessage);
            }
        }

        public void ValidateParameterIsNotMissing(object parameter, string parameterName, ErrorName errorId)
        {
            if (parameter == null)
            {
                var serverMessage = $"Parameter ({parameterName}) is missing!";
                throw new ApiException(errorId, serverMessage);
            }
        }

        public void ValidateInputEnum<TEnum>(string inputString,
            string enumParameterName, ErrorName errorId) where TEnum : struct
        {
            if (string.IsNullOrWhiteSpace(inputString))
                return;

            if (!Enum.TryParse(inputString, true, out TEnum result))
            {
                var serverMessage = $"Parameter ({enumParameterName}), which is enum {typeof(TEnum).Name}, has illegal value: {inputString}!";
                throw new ApiException(errorId, serverMessage);
            }
        }

        public ulong? ValidateEncodedIdFormat(string encodedId, string parameterName = null,
            bool throwException = true)
        {
            if (IdGenerator.IsEncodedIdFormatValid(encodedId))
                return encodedId.IdDecode();

            if (throwException)
            {
                var serverMessage = $"Id ({encodedId}) in parameter ({parameterName}) is not valid!";
                throw new ApiException(ErrorName.ENCODED_ID_FORMAT_IS_NOT_VALID, serverMessage);
            }
            return default;
        }

        public void ValidateUrlStringFormat(string urlString)
        {
            if (!urlString.IsUrlStringValid())
            {
                var serverMessage = $"Invalid url format! ({urlString})!";
                throw new ApiException(ErrorName.URL_FORMAT_IS_NOT_VALID, serverMessage);
            }
        }

        //public ulong? ValidateEncodedIdFormatValidIfExists(string encodedId, string parameterName)
        //{
        //    if (string.IsNullOrWhiteSpace(encodedId))
        //        return default;
        //    return ValidateEncodedIdFormat(encodedId, parameterName);
        //}

        //private void ValidateObjectParametersAreNotNull(ErrorId errorCode, object obj, params string[] exceptions)
        //{
        //    var properties = obj.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

        //    foreach (var prop in properties)
        //    {
        //        if (exceptions.Contains(prop.Name))
        //            continue;

        //        var value = prop.GetValue(obj, null);
        //        //var isEmpty = false;
        //        //switch(value)
        //        //{
        //        //    case string str:
        //        //        if (string.IsNullOrWhiteSpace(str))
        //        //            isEmpty = true;
        //        //        break;
        //        //    default:
        //        //        if(value == null)
        //        //            isEmpty = true;
        //        //        break;
        //        //}

        //        if (value == null)
        //        {
        //            var field = prop.Name.ToSnakeCase();
        //            throw new ApiException(errorCode, $"Field {field} is null.");
        //        }
        //    }
        //}

        //public void ValidateBodyParametersAreNotNull(object obj, params string[] exceptions)
        //    => ValidateObjectParametersAreNotNull(ErrorId.BODY_FIELD_MISSING, obj, exceptions);

        //public void ValidateRouteParametersAreNotNull(object obj, params string[] exceptions)
        //    => ValidateObjectParametersAreNotNull(ErrorId.ROUTE_FIELD_MISSING, obj, exceptions);

        //public void ValidateQueryParametersAreNotNull(object obj, params string[] exceptions)
        //    => ValidateObjectParametersAreNotNull(ErrorId.QUERY_FIELD_MISSING, obj, exceptions);

        //public void ValidateRouteFieldEqualsToBodyField(object routeFieldValue, object bodyFieldValue, string bodyField)
        //{
        //    if (Equals(routeFieldValue, bodyFieldValue) == false)
        //    {
        //        var serverMessage = $"route-{bodyField} != body-{bodyField} => {routeFieldValue} != {bodyFieldValue}";
        //        throw new ApiException(ErrorId.URL_NOT_MATCH_BODY, serverMessage, bodyField);
        //    }
        //}
        //public void ValidatePriceIsNotNegative(long price, string field)
        //{
        //    if (price < 0)
        //    {
        //        var serverMessage = $"{field} has negative value => {price}";
        //        throw new ApiException(ErrorId.NEGATIVE_PRICE, serverMessage, field);
        //    }
        //}
    }
}
