using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Operators;
using FireplaceApi.Domain.Tools;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Validators;

public class AccessTokenValidator : DomainValidator
{
    private readonly ILogger<AccessTokenValidator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly AccessTokenOperator _accessTokenOperator;

    public AccessTokenValidator(ILogger<AccessTokenValidator> logger,
        IServiceProvider serviceProvider, AccessTokenOperator accessTokenOperator)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _accessTokenOperator = accessTokenOperator;
    }

    public void ValidateAccessTokenValueFormat(string value)
    {
        if (Regexes.AccessTokenValue.IsMatch(value) == false)
            throw new AccessTokenAuthenticationFailedException(value);
    }

    public async Task<AccessToken> ValidateAccessTokenValueExistsAsync(string value)
    {
        var accessToken = await _accessTokenOperator
            .GetAccessTokenByValueAsync(value, true);

        if (accessToken == null)
            throw new AccessTokenNotExistException(value);

        return accessToken;
    }

    public async Task ValidateUserCanAccessToAccessTokenValue(User requestingUser, string value)
    {
        var accessToken = await _accessTokenOperator.GetAccessTokenByValueAsync(value);
        if (accessToken.UserId != requestingUser.Id)
            throw new AccessTokenAccessDeniedException(requestingUser.Id, value);
    }
}
