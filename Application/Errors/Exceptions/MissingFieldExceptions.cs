using FireplaceApi.Domain.Errors;

namespace FireplaceApi.Application.Errors;

public class CommunityIdOrNameMissingFieldException : MissingFieldException
{
    public CommunityIdOrNameMissingFieldException()
        : base(
              errorField: FieldName.COMMUNITY_ID_OR_NAME
        )
    { }
}

public class ListOfIdsMissingFieldException : MissingFieldException
{
    public ListOfIdsMissingFieldException()
        : base(
              errorField: FieldName.LIST_OF_IDS
        )
    { }
}

