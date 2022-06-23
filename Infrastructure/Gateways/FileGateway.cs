using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Gateways
{
    public class FileGateway : IFileGateway
    {
        private readonly ILogger<FileGateway> _logger;

        public FileGateway(ILogger<FileGateway> logger)
        {
            _logger = logger;
        }

        public async Task CreateFileAsync(IFormFile formFile, string filePhysicalPath)
        {
            Utils.CreateParentDirectoriesOfFileIfNotExists(filePhysicalPath);
            using var stream = new System.IO.FileStream(filePhysicalPath, System.IO.FileMode.Create);
            await formFile.CopyToAsync(stream);
        }
    }
}
