using FireplaceApi.Application.Models;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Enums;

namespace FireplaceApi.Infrastructure.Converters;

public static class ErrorConverter
{
    public static ErrorEntity ToEntity(this Error error)
    {
        if (error == null)
            return null;

        var errorEntity = new ErrorEntity(error.Id, error.Code, error.Type.Name,
            error.Field.Name, error.ClientMessage, error.HttpStatusCode,
            error.CreationDate, error.ModifiedDate);

        return errorEntity;
    }

    public static Error ToModel(this ErrorEntity errorEntity)
    {
        if (errorEntity == null)
            return null;

        var error = new Error(errorEntity.Id, errorEntity.Code, InfrastructureErrorType.FromName(errorEntity.Type),
            InfrastructureFieldName.FromName(errorEntity.Field), errorEntity.ClientMessage, errorEntity.HttpStatusCode,
            errorEntity.CreationDate, errorEntity.ModifiedDate);

        return error;
    }
}
