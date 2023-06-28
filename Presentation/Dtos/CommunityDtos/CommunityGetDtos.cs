using FireplaceApi.Domain.Communities;
using FireplaceApi.Presentation.Swagger;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Presentation.Dtos;

public class ListCommunitiesInputQueryDto
{
    [FromQuery(Name = "search")]
    public string Search { get; set; }

    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(CommunitySortType))]
    public string SortString { get; set; }

    [FromQuery(Name = "ids")]
    public string EncodedIds { get; set; }
}

public class ListJoinedCommunitiesInputQueryDto
{
    [FromQuery(Name = "sort")]
    [SwaggerEnum(Type = typeof(CommunitySortType))]
    public string SortString { get; set; }
}

public class GetCommunityByIdOrNameInputRouteDto
{
    [Required]
    [FromRoute(Name = "id-or-name")]
    public string EncodedIdOrName { get; set; }
}
