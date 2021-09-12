using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Tools;
using FireplaceApi.Core.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Interfaces;

namespace FireplaceApi.Core.Operators
{
    public class FileOperator
    {
        private readonly ILogger<FileOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly IFileRepository _fileRepository;
        private readonly IFileGateway _fileGateway;
        private readonly Uri _baseUri;
        private readonly string _basePhysicalPath;

        public FileOperator(ILogger<FileOperator> logger, IConfiguration configuration,
            IServiceProvider serviceProvider, IFileRepository fileRepository, 
            IFileGateway fileGateway)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _fileRepository = fileRepository;
            _fileGateway = fileGateway;
            _baseUri = new Uri(_configuration.GetValue<string>(Constants.FilesBaseUrlPathKey));
            _basePhysicalPath = _configuration.GetValue<string>(Constants.FilesBasePhysicalPathKey);
        }

        public async Task<List<File>> ListFilesAsync()
        {
            var file = await _fileRepository.ListFilesAsync();
            return file;
        }

        public async Task<File> GetFileByIdAsync(long id)
        {
            var file = await _fileRepository.GetFileByIdAsync(id);
            if (file == null)
                return file;

            return file;
        }

        public async Task<File> CreateFileAsync(IFormFile formFile)
        {
            var realName = formFile.FileName;
            var extension = System.IO.Path.GetExtension(realName);
            var name = await GenerateUniqueFileNameAsync(extension);
            var relativePath = name;
            var uri = new Uri(_baseUri, relativePath);
            var physicalPath = System.IO.Path.GetFullPath(relativePath, _basePhysicalPath);
            var file = await _fileRepository.CreateFileAsync(name, realName, uri, physicalPath);
            await _fileGateway.CreateFileAsync(formFile, physicalPath);
            _logger.LogInformation($"New uploaded file: {file.ToJson()}");

            file = await GetFileByIdAsync(file.Id);
            return file;
        }

        public async Task<File> PatchFileByIdAsync(long id, string name = null,
            string realName = null, Uri uri = null, string physicalPath = null)
        {
            var file = await _fileRepository.GetFileByIdAsync(id);
            file = await ApplyFileChangesAsync(file, name, realName, uri, physicalPath);
            file = await GetFileByIdAsync(file.Id);
            return file;
        }

        public async Task DeleteFileAsync(long id)
        {
            await _fileRepository.DeleteFileAsync(id);
        }

        public async Task<bool> DoesFileIdExistAsync(long id)
        {
            var fileIdExists = await _fileRepository.DoesFileIdExistAsync(id);
            return fileIdExists;
        }

        public async Task<bool> DoesFileNameExistAsync(string name)
        {
            var fileIdExists = await _fileRepository.DoesFileNameExistAsync(name);
            return fileIdExists;
        }


        public async Task<File> ApplyFileChangesAsync(File file, string name = null,
            string realName = null, Uri uri = null, string physicalPath = null)
        {
            if (name != null)
            {
                file.Name = name;
            }

            if (realName != null)
            {
                file.RealName = realName;
            }

            if (uri != null)
            {
                file.Uri = uri;
            }

            if (physicalPath != null)
            {
                file.PhysicalPath = physicalPath;
            }

            file = await _fileRepository.UpdateFileAsync(file);
            return file;
        }

        private async Task<string> GenerateUniqueFileNameAsync(string extension)
        {
            string fileName;
            do
            {
                fileName = Utils.RandomString(Constants.FileNameLength) + extension;
            } while (await DoesFileNameExistAsync(fileName));

            return fileName;
        }
    }
}
