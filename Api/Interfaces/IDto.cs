using Microsoft.OpenApi.Any;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Api.Interfaces
{
    public interface IDto<TModel>
    {
        public TModel ToModel();
        public Dictionary<string, IOpenApiAny> GetExamples();
    }
}
