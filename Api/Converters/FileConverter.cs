using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Api.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Api.Controllers;

namespace FireplaceApi.Api.Converters
{
    public class FileConverter
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
            _baseUri = new Uri(_configuration.GetValue<string>(Constants.FilesBaseUrlPathKey));
            _basePhysicalPath = _configuration.GetValue<string>(Constants.FilesBasePhysicalPathKey);
        }

        public FileDto ConvertToDto(File file)
        {
            if (file == null)
                return null;

            var fileDto = new FileDto(file.Id, file.Uri.AbsoluteUri, file.CreationDate);           

            return fileDto;
        }
    }
}
