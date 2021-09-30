using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Api.Converters
{
    public static class PageConverter
    {
        public static PaginationInputParameters ConvertToModel(PaginationInputQueryParameters inputQueryParameters)
        {
            var model = new PaginationInputParameters(inputQueryParameters.Limit,
                inputQueryParameters.Pointer, inputQueryParameters.Next, 
                inputQueryParameters.Previous, inputQueryParameters.Page,
                inputQueryParameters.Offset);

            return model;
        }
    }
}
