using FireplaceApi.Domain.Enums;
using System.Runtime.CompilerServices;

namespace FireplaceApi.Application.Enums
{
    public sealed class ApplicationErrorType : ErrorType
    {
        public static readonly ApplicationErrorType MISSING_FIELD = new();

        private ApplicationErrorType([CallerMemberName] string name = null) : base(name) { }
    }
}
