using FireplaceApi.Application.Communities;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Presentation.Auth;
using FireplaceApi.Presentation.Converters;
using FireplaceApi.Presentation.Dtos;
using FireplaceApi.Presentation.Interfaces;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.GraphQL;

[ExtendObjectType(typeof(GraphQLMutation))]
public class CommunityMutationResolvers
{
    public async Task<CommunityDto> CreateCommunitiesAsync(
        [Service(ServiceKind.Resolver)] CommunityService communityService,
        [Service] IServiceProvider serviceProvider,
        [User] RequestingUser requestingUser,
        [GraphQLNonNullType] CreateCommunityInput input)
    {
        input.Validate(serviceProvider);
        var community = await communityService.CreateCommunityAsync(requestingUser.Id.Value, input.Name);
        var communityDto = community.ToDto();
        return communityDto;
    }
}

public class CreateCommunityInput : IValidator
{
    public string Name { get; set; }

    public void Validate(IServiceProvider serviceProvider)
    {
        var presentationValidator = serviceProvider.GetService<Validators.CommunityValidator>();
        var applicationValidator = presentationValidator.ApplicationValidator;

        presentationValidator.ValidateFieldIsNotMissing(Name, FieldName.COMMUNITY_NAME);
        applicationValidator.ValidateCommunityNameFormat(Name);
    }
}
