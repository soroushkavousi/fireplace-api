﻿using FireplaceApi.Application.Dtos;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.ValueObjects;

namespace FireplaceApi.Application.Converters;

public static class ErrorConverter
{
    public static ErrorDto ToDto(this Error error)
    {
        if (error == null)
            return null;

        var errorDto = new ErrorDto(error.Code, error.Type.Name,
            error.Field.Name, error.ClientMessage, error.HttpStatusCode);

        return errorDto;
    }

    public static ApiExceptionErrorDto ToApiExceptionDto(this Error error)
    {
        if (error == null)
            return null;

        var errorDto = new ApiExceptionErrorDto(error.Code, error.Type.Name,
            error.Field.Name, error.ClientMessage);

        return errorDto;
    }

    public static QueryResultDto<ErrorDto> ToDto(this QueryResult<Error> queryResult)
        => queryResult.ToDto(ToDto);
}
