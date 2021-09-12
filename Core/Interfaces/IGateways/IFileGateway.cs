using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IFileGateway
    {
        public Task CreateFileAsync(IFormFile formFile, string filePhysicalPath);
    }
}
