using FireplaceApi.Api.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FireplaceApi.Api.Controllers
{
    public class ControllerListUsersInputQueryParameters
    {
        [FromQuery(Name = "include_email")]
        public bool IncludeEmail { get; set; } = true;
        [FromQuery(Name = "include_sessions")]
        public bool IncludeSessions { get; set; } = false;
    }

    public class ControllerListUsersOutputHeaderParameters : IControllerOutputHeaderParameters
    {
        public HeaderDictionary GetHeaderDictionary()
        {
            return default;
        }
    }
}
