using Microsoft.OpenApi.Any;
using System.Collections.Generic;

namespace FireplaceApi.Application.Interfaces
{
    public interface IDto<TModel>
    {
        public TModel ToModel();
        public Dictionary<string, IOpenApiAny> GetExamples();
    }
}
