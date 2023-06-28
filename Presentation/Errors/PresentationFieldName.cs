using FireplaceApi.Infrastructure.Errors;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Presentation.Errors;

public class PresentationFieldName : InfrastructureFieldName
{
    private PresentationFieldName([CallerMemberName] string name = null) : base(name) { }
}
