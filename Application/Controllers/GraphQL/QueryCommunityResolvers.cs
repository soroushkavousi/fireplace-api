using FireplaceApi.Application.Converters;
using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Services;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Controllers
{
    [ExtendObjectType(typeof(GraphQLController))]
    public class QueryCommunityResolvers
    {
        public async Task<QueryResultDto<CommunityDto>> GetCommunitiesAsync([Service] ILogger<QueryCommunityResolvers> logger,
            [Service] CommunityConverter communityConverter, [Service] CommunityService communityService,
            string name, SortType? sort = null)
        {
            var queryResult = await communityService.ListCommunitiesAsync(name, sort);
            var queryResultDto = communityConverter.ConvertToDto(queryResult);
            return queryResultDto;
        }
    }
}
