using FireplaceApi.Domain.Errors;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Errors;

public class ErrorValidator : ApplicationValidator
{
    private readonly ILogger<ErrorValidator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly ErrorOperator _errorOperator;

    public ErrorValidator(ILogger<ErrorValidator> logger,
        IServiceProvider serviceProvider, ErrorOperator errorOperator)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _errorOperator = errorOperator;
    }

    public async Task ValidateListErrorsInputParametersAsync(ulong userId)
    {
        await Task.CompletedTask;
    }

    public async Task ValidateGetErrorByCodeInputParametersAsync(ulong userId, ErrorIdentifier identifier)
    {
        await ValidateErrorCodeExists(identifier);
    }

    public async Task ValidateErrorCodeExists(ErrorIdentifier identifier)
    {
        if (await _errorOperator.DoesErrorExistAsync(identifier) == false)
            throw new ErrorNotExistException(identifier);
    }

    public void ValidateErrorCodeFormat(int code)
    {

    }
}
