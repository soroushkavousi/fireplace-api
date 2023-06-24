using FireplaceApi.Domain.Users;
using FireplaceApi.Presentation.Auth;
using HotChocolate;
using HotChocolate.AspNetCore;
using HotChocolate.Execution;
using Microsoft.AspNetCore.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Presentation.GraphQL;

public class RequestingUserGlobalState : DefaultHttpRequestInterceptor
{
    public override ValueTask OnCreateAsync(HttpContext context,
        IRequestExecutor requestExecutor, IQueryRequestBuilder requestBuilder,
        CancellationToken cancellationToken)
    {
        var user = (User)context.Items.GetValue(AuthConstants.RequestingUserKey);
        requestBuilder.SetGlobalState("User", user);

        return base.OnCreateAsync(context, requestExecutor, requestBuilder,
            cancellationToken);
    }
}

public class UserAttribute : GlobalStateAttribute
{
    public UserAttribute() : base("User")
    {

    }
}
