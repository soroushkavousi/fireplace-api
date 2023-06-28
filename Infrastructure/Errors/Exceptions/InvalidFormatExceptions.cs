using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Infrastructure.Errors;

public class EncodedIdInvalidFormatException : ApiException
{
    public EncodedIdInvalidFormatException(string encodedId)
        : base(
            errorType: ErrorType.INVALID_FORMAT,
            errorField: InfrastructureFieldName.ENCODED_ID,
            errorServerMessage: "The encoded id is not valid!!",
            parameters: new { encodedId },
            systemException: null
        )
    { }
}
