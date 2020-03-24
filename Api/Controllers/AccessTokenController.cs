using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Api.Controllers.Parameters;
using GamingCommunityApi.Api.Controllers.Parameters.ErrorParameters;
using GamingCommunityApi.Api.Converters;
using GamingCommunityApi.Api.Extensions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using GamingCommunityApi.Api.Controllers.Parameters.AccessTokenParameters;
using Microsoft.AspNetCore.Authorization;
using GamingCommunityApi.Core.Services;
using GamingCommunityApi.Core.Models.UserInformations;

namespace GamingCommunityApi.Api.Controllers
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
