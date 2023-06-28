using FireplaceApi.Domain.Errors;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Errors;

public class ErrorService
{
    private readonly IServerLogger<ErrorService> _logger;
    private readonly ErrorValidator _errorValidator;
    private readonly ErrorOperator _errorOperator;

    public ErrorService(IServerLogger<ErrorService> logger, ErrorValidator errorValidator,
        ErrorOperator errorOperator)
    {
        _logger = logger;
        _errorValidator = errorValidator;
        _errorOperator = errorOperator;
    }

    public async Task<List<Error>> ListErrorsAsync(ulong userId)
    {
        await _errorValidator.ValidateListErrorsInputParametersAsync(userId);
        var errors = await _errorOperator.ListErrorsAsync();
        return errors;
    }

    public async Task<Error> GetErrorAsync(ulong userId, ErrorIdentifier identifier)
    {
        await _errorValidator.ValidateGetErrorByCodeInputParametersAsync(userId, identifier);
        var error = await _errorOperator.GetErrorAsync(identifier);
        return error;
    }
}
