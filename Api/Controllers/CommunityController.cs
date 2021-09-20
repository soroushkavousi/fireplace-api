using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Api.Controllers;
using FireplaceApi.Api.Extensions;
using Microsoft.AspNetCore.JsonPatch;
using FireplaceApi.Api.Converters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using FireplaceApi.Core.Services;
using FireplaceApi.Core.ValueObjects;
using FireplaceApi.Core.Extensions;
using System.Diagnostics;
using FireplaceApi.Core.Models;

namespace FireplaceApi.Api.Controllers
{
    [ApiController]
    [ApiVersion("0.1")]
    [Route("v{version:apiVersion}/communities")]
    [Produces("application/json")]
    public class CommunityController : ApiController
    {
        private readonly ILogger<CommunityController> _logger;
        private readonly CommunityConverter _communityConverter;
        private readonly CommunityService _communityService;

        public CommunityController(ILogger<CommunityController> logger,
            CommunityConverter communityConverter, CommunityService communityService)
        {
            _logger = logger;
            _communityConverter = communityConverter;
            _communityService = communityService;
        }

        /// <summary>
        /// List all communities.
        /// </summary>
        /// <returns>List of communities</returns>
        /// <response code="200">All communities was successfully retrieved.</response>
        [HttpGet]
        [ProducesResponseType(typeof(PageDto<CommunityDto>), StatusCodes.Status200OK)]
        public async Task<ActionResult<PageDto<CommunityDto>>> ListCommunitiesAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromQuery] ControllerListCommunitiesInputQueryParameters inputQueryParameters)
        {
            //var accessTokenValue = FindAccessTokenValue(inputHeaderParameters, inputCookieParameters);
            var page = await _communityService.ListCommunitiesAsync(requesterUser);
            var pageDto = _communityConverter.ConvertToDto(page);
            //SetOutputHeaderParameters(communityDtos.HeaderParameters);
            return pageDto;
        }

        /// <summary>
        /// Get a single community.
        /// </summary>
        /// <returns>Requested community</returns>
        /// <response code="200">The community was successfully retrieved.</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityDto>> GetCommunityByIdAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerGetCommunityByIdInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetCommunityByIdInputQueryParameters inputQueryParameters)
        {
            var community = await _communityService
                .GetCommunityByIdAsync(requesterUser, inputRouteParameters.Id, 
                inputQueryParameters.IncludeCreator);
            var communityDto = _communityConverter.ConvertToDto(community);
            return communityDto;
        }

        /// <summary>
        /// Get a single community.
        /// </summary>
        /// <returns>Requested community</returns>
        /// <response code="200">The community was successfully retrieved.</response>
        [HttpGet("{name}")]
        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityDto>> GetCommunityByNameAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerGetCommunityByNameInputRouteParameters inputRouteParameters,
            [FromQuery] ControllerGetCommunityByNameInputQueryParameters inputQueryParameters)
        {
            var community = await _communityService
                .GetCommunityByNameAsync(requesterUser, inputRouteParameters.Name,
                inputQueryParameters.IncludeCreator);
            var communityDto = _communityConverter.ConvertToDto(community);
            return communityDto;
        }

        /// <summary>
        /// Create a community.
        /// </summary>
        /// <returns>Created community</returns>
        /// <response code="200">Returns the newly created item</response>
        [HttpPost]
        [Consumes("application/json")]
        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityDto>> CreateCommunityAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromBody] ControllerCreateCommunityInputBodyParameters inputBodyParameters)
        {
            var community = await _communityService.CreateCommunityAsync(inputBodyParameters.Name,
                requesterUser.Id);
            var communityDto = _communityConverter.ConvertToDto(community);
            return communityDto;
        }

        /// <summary>
        /// Update a single community.
        /// </summary>
        /// <returns>Updated community</returns>
        /// <response code="200">The community was successfully updated.</response>
        [HttpPatch("{id}")]
        [Consumes("application/merge-patch+json")]
        [ProducesResponseType(typeof(CommunityDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<CommunityDto>> PatchCommunityAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerPatchCommunityInputRouteParameters inputRouteParameters,
            [FromBody] ControllerPatchCommunityInputBodyParameters inputBodyParameters)
        {
            var community = await _communityService.PatchCommunityByIdAsync(requesterUser, inputRouteParameters.Id);
            var communityDto = _communityConverter.ConvertToDto(community);
            return communityDto;
        }

        /// <summary>
        /// Delete a single community.
        /// </summary>
        /// <returns>No content</returns>
        /// <response code="200">The community was successfully deleted.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> DeleteCommunityAsync(
            [BindNever][FromHeader] User requesterUser,
            [FromRoute] ControllerDeleteCommunityInputRouteParameters inputRouteParameters)
        {
            await _communityService.DeleteCommunityAsync(requesterUser, inputRouteParameters.Id);
            return Ok();
        }
    }
}
