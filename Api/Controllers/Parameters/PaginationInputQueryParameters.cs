using Microsoft.AspNetCore.Http;
using FireplaceApi.Api.Extensions;
using FireplaceApi.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Primitives;
using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class PaginationInputQueryParameters
    {
        [FromQuery(Name = "limit")]
        public int? Limit { get; set; }

        [FromQuery(Name = "pointer")]
        public string Pointer { get; set; }

        [FromQuery(Name = "next")]
        public bool? Next { get; set; }

        [FromQuery(Name = "previous")]
        public bool? Previous { get; set; }

        [FromQuery(Name = "page")]
        public int? Page { get; set; }

        [FromQuery(Name = "offset")]
        public int? Offset { get; set; }
    }
}
