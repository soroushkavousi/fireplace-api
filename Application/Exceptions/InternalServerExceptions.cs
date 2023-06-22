using FireplaceApi.Application.Enums;
using System;

namespace FireplaceApi.Application.Exceptions;

public class InternalServerException : ApiException
{
    public InternalServerException(string errorServerMessage = null, object parameters = null,
        Exception systemException = null)
        : base(
            errorType: ErrorType.INTERNAL_SERVER,
            errorField: FieldName.GENERAL,
            errorServerMessage: errorServerMessage,
            parameters: parameters,
            systemException: systemException
        )
    { }
}
