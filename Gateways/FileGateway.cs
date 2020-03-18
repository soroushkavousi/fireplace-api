using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GamingCommunityApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GamingCommunityApi.Tools;

namespace GamingCommunityApi.Gateways
{
    public class FileGateway
    {
        private readonly ILogger<FileGateway> _logger;
        private readonly IConfiguration _configuration;

        public FileGateway(ILogger<FileGateway> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public async Task CreateFileAsync(IFormFile formFile, string filePhysicalPath)
        {
            Utils.CreateParentDirectoriesOfFileIfNotExists(filePhysicalPath);
            using var stream = new System.IO.FileStream(filePhysicalPath, System.IO.FileMode.Create);
            await formFile.CopyToAsync(stream);
        }
    }
}
