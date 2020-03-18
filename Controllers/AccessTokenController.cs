using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Controllers.Parameters;
using GamingCommunityApi.Controllers.Parameters.ErrorParameters;
using GamingCommunityApi.Models;
using GamingCommunityApi.Repositories;
using GamingCommunityApi.Validators;
using GamingCommunityApi.Services;
using GamingCommunityApi.Converters;
using GamingCommunityApi.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using GamingCommunityApi.Controllers.Parameters.AccessTokenParameters;
using GamingCommunityApi.Models.UserInformations;
using Microsoft.AspNetCore.Authorization;

namespace GamingCommunityApi.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("v{version:apiVersion}/accessTokens")]
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
            [BindNever] [FromHeader] User requesterUser,
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
