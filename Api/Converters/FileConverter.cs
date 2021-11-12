using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Api.Converters
{
    public class FileConverter : BaseConverter<File, FileDto>
    {
        private readonly ILogger<FileConverter> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly Uri _baseUri;
        private readonly string _basePhysicalPath;

        public FileConverter(ILogger<FileConverter> logger,
            IServiceProvider serviceProvider, IConfiguration configuration)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _baseUri = new Uri(_configuration.GetValue<string>(Tools.Constants.FilesBaseUrlPathKey));
            _basePhysicalPath = _configuration.GetValue<string>(Tools.Constants.FilesBasePhysicalPathKey);
        }

        public override FileDto ConvertToDto(File file)
        {
            if (file == null)
                return null;

            var fileDto = new FileDto(file.Id.IdEncode(), file.Uri.AbsoluteUri, file.CreationDate);

            return fileDto;
        }
    }
}
