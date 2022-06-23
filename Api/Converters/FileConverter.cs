using FireplaceApi.Api.Controllers;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Api.Converters
{
    public class FileConverter : BaseConverter<File, FileDto>
    {
        private readonly ILogger<FileConverter> _logger;
        private readonly Uri _baseUri;
        private readonly string _basePhysicalPath;

        public FileConverter(ILogger<FileConverter> logger)
        {
            _logger = logger;
            _baseUri = new Uri(Configs.Instance.File.BaseUrlPath);
            _basePhysicalPath = Configs.Instance.File.BasePhysicalPath;
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
