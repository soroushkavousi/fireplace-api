using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Infrastructure.Entities;
using FireplaceApi.Infrastructure.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Models;

namespace FireplaceApi.Infrastructure.Converters
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

        public FileEntity ConvertToEntity(File file)
        {
            if (file == null)
                return null;

            var relativeUri = GetRelativeUri(file.Uri).ToString();
            var relativePhysicalPath = GetRelativePhysicalPath(file.PhysicalPath);

            var fileEntity = new FileEntity(file.Name, file.RealName,
                relativeUri, relativePhysicalPath, file.CreationDate, 
                file.ModifiedDate, file.Id);                

            return fileEntity;
        }

        public File ConvertToModel(FileEntity fileEntity)
        {
            if (fileEntity == null)
                return null;

            var uri = GetAbsoluteUri(new Uri(fileEntity.RelativeUri, UriKind.Relative));
            var physicalPath = GetAbsolutePhysicalPath(fileEntity.RelativePhysicalPath);

            var file = new File(fileEntity.Id.Value, fileEntity.Name, fileEntity.RealName,
                uri, physicalPath, fileEntity.CreationDate, fileEntity.ModifiedDate);

            return file;
        }

        public Uri GetRelativeUri(Uri absoluteUri)
        {
            var relativeUri = _baseUri.MakeRelativeUri(absoluteUri);
            return relativeUri;
        }

        public Uri GetAbsoluteUri(Uri relativeUri)
        {
            var absoluteUri = new Uri(_baseUri, relativeUri);
            return absoluteUri;
        }

        public string GetRelativePhysicalPath(string absolutePhysicalPath)
        {
            var relativePhysicalPath = System.IO.Path.GetRelativePath(_basePhysicalPath, absolutePhysicalPath);
            return relativePhysicalPath;
        }

        public string GetAbsolutePhysicalPath(string relativePhysicalPath)
        {
            var absolutePhysicalPath = System.IO.Path.GetFullPath(relativePhysicalPath, _basePhysicalPath);
            return absolutePhysicalPath;
        }
    }
}
