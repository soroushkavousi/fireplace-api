using FireplaceApi.Domain.Errors;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Errors;

public class ErrorOperator
{
    private readonly IServerLogger<ErrorOperator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IErrorRepository _errorRepository;

    public ErrorOperator(IServerLogger<ErrorOperator> logger,
        IServiceProvider serviceProvider, IErrorRepository errorRepository)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _errorRepository = errorRepository;
    }

    public async Task<List<Error>> ListErrorsAsync()
    {
        var error = await _errorRepository.ListErrorsAsync();
        return error;
    }

    public async Task<Error> GetErrorAsync(ErrorIdentifier identifier)
    {
        var error = await _errorRepository.GetErrorAsync(identifier);
        if (error == null)
            return error;

        return error;
    }

    public async Task<Error> GetErrorAsync(Exception exception)
    {
        var sw = Stopwatch.StartNew();
        var apiException = exception switch
        {
            ApiException apiExceptionObject => apiExceptionObject,
            _ => new InternalServerException(Error.InternalServerError.ServerMessage, systemException: exception),
        };
        var error = await _errorRepository.GetErrorAsync(apiException.ErrorIdentifier);
        if (error == null)
        {
            _logger.LogServerError("Can't fill error details from database!", sw, parameters: apiException);
            var errorTypeGeneralIdentifier = ErrorIdentifier.OfTypeAndField(apiException.ErrorType, FieldName.GENERAL);
            error = await _errorRepository.GetErrorAsync(errorTypeGeneralIdentifier);
            error ??= Error.InternalServerError;
        }

        error.Field = apiException.ErrorField;
        error.ServerMessage = apiException.ErrorServerMessage;
        error.Exception = apiException.Exception;
        error.Parameters = apiException.Parameters;
        return error;
    }

    public async Task<Error> CreateErrorAsync(int code, ErrorType type,
        FieldName field, string clientMessage, int httpStatusCode)
    {
        var error = await _errorRepository.CreateErrorAsync(code,
            type, field, clientMessage, httpStatusCode);
        return error;
    }

    public async Task<Error> PatchErrorAsync(ErrorIdentifier identifier, int? code = null,
        ErrorType type = null, FieldName field = null, string clientMessage = null,
        int? httpStatusCode = null)
    {
        var error = await _errorRepository.GetErrorAsync(identifier);
        error = await ApplyErrorChanges(error, code, type, field, clientMessage, httpStatusCode);
        error = await GetErrorAsync(identifier);
        return error;
    }

    public async Task DeleteErrorAsync(ErrorIdentifier identifier)
    {
        await _errorRepository.DeleteErrorAsync(identifier);

    }

    public async Task<bool> DoesErrorExistAsync(ErrorIdentifier identifier)
    {
        var errorIdExists = await _errorRepository.DoesErrorExistAsync(identifier);
        return errorIdExists;
    }

    private async Task<Error> ApplyErrorChanges(Error error, int? code = null,
        ErrorType type = null, FieldName field = null,
        string clientMessage = null, int? httpStatusCode = null)
    {
        if (code != null)
        {
            error.Code = code.Value;
        }

        if (type != null)
        {
            error.Type = type;
        }

        if (field != null)
        {
            error.Field = field;
        }

        if (clientMessage != null)
        {
            error.ClientMessage = clientMessage;
        }

        if (httpStatusCode != null)
        {
            error.HttpStatusCode = httpStatusCode.Value;
        }

        error = await _errorRepository.UpdateErrorAsync(error);
        return error;
    }
}
