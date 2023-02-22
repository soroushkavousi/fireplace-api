using FireplaceApi.Api.Converters;
using FireplaceApi.Core.Services;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Controllers
{
    [ExtendObjectType(typeof(GraphQLController))]
    public class QueryCommunityResolvers
    {
        public async Task<QueryResultDto<CommunityDto>> GetCommunitiesAsync([Service] ILogger<QueryCommunityResolvers> logger,
            [Service] CommunityConverter communityConverter, [Service] CommunityService communityService,
            string name, string sort = null)
        {
            var queryResult = await communityService.ListCommunitiesAsync(name, sort);
            var queryResultDto = communityConverter.ConvertToDto(queryResult);
            return queryResultDto;
        }
    }
}
