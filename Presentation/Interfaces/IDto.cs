using Microsoft.OpenApi.Any;
using System.Collections.Generic;

namespace FireplaceApi.Presentation.Interfaces;

public interface IDto<TModel>
{
    public TModel ToModel();
    public Dictionary<string, IOpenApiAny> GetExamples();
}
