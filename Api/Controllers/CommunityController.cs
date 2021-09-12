//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using FireplaceApi.Api.Controllers;
//using FireplaceApi.Api.Controllers.Parameters.CommunityParameters;
//using FireplaceApi.Api.Extensions;
//using FireplaceApi.Api.Controllers.Parameters.ErrorParameters;
//using Microsoft.AspNetCore.JsonPatch;
//using FireplaceApi.Api.Converters;
//using Microsoft.AspNetCore.Mvc.ModelBinding;
//using Microsoft.AspNetCore.Authorization;
//using FireplaceApi.Core.Services;
//using FireplaceApi.Core.ValueObjects;
//using FireplaceApi.Core.Models.CommunityInformations;
//using FireplaceApi.Core.Extensions;
//using System.Diagnostics;

//namespace FireplaceApi.Api.Controllers
//{
//    [ApiController]
//    [ApiVersion("0.1")]
//    [Route("v{version:apiVersion}/communities")]
//    [Produces("application/json")]
//    public class CommunityController : ApiController
//    {
//        private readonly ILogger<CommunityController> _logger;
//        private readonly CommunityConverter _communityConverter;
//        private readonly CommunityService _communityService;

//        public CommunityController(ILogger<CommunityController> logger, CommunityConverter communityConverter, CommunityService communityService)
//        {
//            _logger = logger;
//            _communityConverter = communityConverter;
//            _communityService = communityService;
//        }

//        /// <summary>
//        /// Sign up with email.
//        /// </summary>
//        /// <returns>Created community</returns>
//        /// <response code="200">Returns the newly created item</response>
//        [AllowAnonymous]
//        [HttpPost("sign-up-with-email")]
//        [Consumes("application/json")]
//        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
//        public async Task<ActionResult<CommunityDto>> CreateCommunityAsync(
//            [BindNever] [FromHeader] ControllerInputHeaderParameters inputHeaderParameters,
//            [FromBody] ControllerSignUpWithEmailInputBodyParameters inputBodyParameters)
//        {
//            var password = Password.OfValue(inputBodyParameters.Password);
//            var community = await _communityService.SignUpWithEmailAsync(inputHeaderParameters.IpAddress, inputBodyParameters.FirstName,
//                inputBodyParameters.LastName, inputBodyParameters.Communityname, password, inputBodyParameters.EmailAddress);
//            var communityDto = _communityConverter.ConvertToDto(community);
//            var outputCookieParameters = new ControllerSignUpWithEmailOutputCookieParameters(communityDto.AccessToken);
//            SetOutputCookieParameters(outputCookieParameters);
//            return communityDto;
//        }


//        /// <summary>
//        /// List all communities.
//        /// </summary>
//        /// <returns>List of communities</returns>
//        /// <response code="200">All communities was successfully retrieved.</response>
//        [HttpGet]
//        [ProducesResponseType(typeof(IEnumerable<CommunityDto>), StatusCodes.Status200OK)]
//        public async Task<ActionResult<IEnumerable<CommunityDto>>> ListCommunitiesAsync(
//            [BindNever] [FromHeader] Community requesterCommunity,
//            [FromQuery] ControllerListCommunitiesInputQueryParameters inputQueryParameters)
//        {
//            //var accessTokenValue = FindAccessTokenValue(inputHeaderParameters, inputCookieParameters);
//            var communities = await _communityService.ListCommunitiesAsync(requesterCommunity, 
//                inputQueryParameters.IncludeEmail, inputQueryParameters.IncludeSessions);
//            var communityDtos = communities.Select(community => _communityConverter.ConvertToDto(community)).ToList();
//            //SetOutputHeaderParameters(communityDtos.HeaderParameters);
//            return communityDtos;
//        }

//        /// <summary>
//        /// Get a single community.
//        /// </summary>
//        /// <returns>Requested community</returns>
//        /// <response code="200">The community was successfully retrieved.</response>
//        [HttpGet("{id}")]
//        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
//        public async Task<ActionResult<CommunityDto>> GetCommunityByIdAsync(
//            [BindNever] [FromHeader] Community requesterCommunity,
//            [FromRoute] ControllerGetCommunityByIdInputRouteParameters inputRouteParameters,
//            [FromQuery] ControllerGetCommunityByIdInputQueryParameters inputQueryParameters)
//        {
//            var sw = Stopwatch.StartNew();
//            var community = await _communityService.GetCommunityByIdAsync(requesterCommunity, inputRouteParameters.Id,
//                inputQueryParameters.IncludeEmail, inputQueryParameters.IncludeSessions);
//            var communityDto = _communityConverter.ConvertToDto(community);
//            _logger.LogTrace(sw);
//            return communityDto;
//        }

//        /// <summary>
//        /// Update a single community.
//        /// </summary>
//        /// <returns>Updated community</returns>
//        /// <response code="200">The community was successfully updated.</response>
//        [HttpPatch("{id}")]
//        [Consumes("application/merge-patch+json")]
//        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
//        public async Task<ActionResult<CommunityDto>> PatchCommunityAsync(
//            [BindNever] [FromHeader] Community requesterCommunity,
//            [FromRoute] ControllerPatchCommunityInputRouteParameters inputRouteParameters,
//            [FromBody] ControllerPatchCommunityInputBodyParameters inputBodyParameters)
//        {
//            var password = Password.OfValue(inputBodyParameters.Password);
//            var oldPassword = Password.OfValue(inputBodyParameters.OldPassword);
//            var community = await _communityService.PatchCommunityByIdAsync(requesterCommunity, inputRouteParameters.Id, inputBodyParameters.FirstName,
//                inputBodyParameters.LastName, inputBodyParameters.Communityname, oldPassword, password, inputBodyParameters.EmailAddress);
//            var communityDto = _communityConverter.ConvertToDto(community);
//            return communityDto;
//        }

//        /// <summary>
//        /// Delete a single community.
//        /// </summary>
//        /// <returns>No content</returns>
//        /// <response code="200">The community was successfully deleted.</response>
//        [HttpDelete("{id}")]
//        [ProducesResponseType(StatusCodes.Status200OK)]
//        public async Task<IActionResult> DeleteCommunityAsync(
//            [BindNever] [FromHeader] Community requesterCommunity,
//            [FromRoute] ControllerDeleteCommunityInputRouteParameters inputRouteParameters)
//        {
//            await _communityService.DeleteCommunityAsync(requesterCommunity, inputRouteParameters.Id);
//            return Ok();
//        }
//    }
//}
