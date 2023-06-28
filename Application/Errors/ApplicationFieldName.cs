using FireplaceApi.Domain.Errors;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Application.Errors;

public class ApplicationFieldName : FieldName
{
    protected ApplicationFieldName([CallerMemberName] string name = null) : base(name) { }
}
