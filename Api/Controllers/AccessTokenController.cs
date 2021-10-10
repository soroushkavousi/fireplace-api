﻿using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("v{version:apiVersion}/access-tokens")]
    [Produces("application/json")]
    public class AccessTokenController : ApiController
    {
        private readonly AccessTokenConverter _accessTokenConverter;
        private readonly AccessTokenService _accessTokenService;

        public AccessTokenController(AccessTokenConverter accessTokenConverter, AccessTokenService accessTokenService)
        {
            _accessTokenConverter = accessTokenConverter;
            _accessTokenService = accessTokenService;
        }

        /// <summary>
        /// Get a single access token.
        /// </summary>
        /// <returns>Requested access token</returns>
        /// <response code="200">The access token was successfully retrieved.</response>
        [HttpGet("{value}")]
        [ProducesResponseType(typeof(AccessTokenDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<AccessTokenDto>> GetAccessTokenByValueAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromQuery] ControllerGetAccessTokenByValueInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetAccessTokenByValueInputQueryParameters inputQueryParameters)
        {
            var accessToken = await _accessTokenService.GetAccessTokenByValueAsync(requesterUser,
                inputRouteParameters.Value, inputQueryParameters.IncludeUser);
            var accessTokenDto = _accessTokenConverter.ConvertToDto(accessToken);
            return accessTokenDto;
        }
    }
}
