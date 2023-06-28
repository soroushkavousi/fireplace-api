using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Common;

public abstract class RequestHandler<TRequest, TResponse> : IRequestHandler<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken)
    {
        await ValidateAsync(request, cancellationToken);
        return await OperateAsync(request, cancellationToken);
    }

    protected abstract Task ValidateAsync(TRequest request, CancellationToken cancellationToken);
    protected abstract Task<TResponse> OperateAsync(TRequest request, CancellationToken cancellationToken);
}
