using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
using FireplaceApi.Domain.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Operators;

public class FileOperator
{
    private readonly ILogger<FileOperator> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly IFileRepository _fileRepository;
    private readonly IFileGateway _fileGateway;
    private readonly Uri _baseUri;
    private readonly string _basePhysicalPath;

    public FileOperator(ILogger<FileOperator> logger,
        IServiceProvider serviceProvider, IFileRepository fileRepository,
        IFileGateway fileGateway)
    {
        _logger = logger;
        _serviceProvider = serviceProvider;
        _fileRepository = fileRepository;
        _fileGateway = fileGateway;
        _baseUri = new Uri(Configs.Current.File.BaseUrlPath);
        _basePhysicalPath = Configs.Current.File.BasePhysicalPath;
    }

    public async Task<List<File>> ListFilesAsync()
    {
        var file = await _fileRepository.ListFilesAsync();
        return file;
    }

    public async Task<File> GetFileByIdAsync(ulong id)
    {
        var file = await _fileRepository.GetFileByIdAsync(id);
        if (file == null)
            return file;

        return file;
    }

    public async Task<File> CreateFileAsync(IFormFile formFile)
    {
        var sw = Stopwatch.StartNew();
        var realName = formFile.FileName;
        var extension = System.IO.Path.GetExtension(realName);
        var name = await GenerateUniqueFileNameAsync(extension);
        var relativePath = name;
        var uri = new Uri(_baseUri, relativePath);
        var physicalPath = System.IO.Path.GetFullPath(relativePath, _basePhysicalPath);
        var id = await IdGenerator.GenerateNewIdAsync(DoesFileIdExistAsync);
        var file = await _fileRepository.CreateFileAsync(id, name, realName,
            uri, physicalPath);
        await _fileGateway.CreateFileAsync(formFile, physicalPath);
        _logger.LogAppInformation($"New uploaded file: {file.ToJson()}", sw);

        file = await GetFileByIdAsync(file.Id);
        return file;
    }

    public async Task<File> PatchFileByIdAsync(ulong id, string name = null,
        string realName = null, Uri uri = null, string physicalPath = null)
    {
        var file = await _fileRepository.GetFileByIdAsync(id);
        file = await ApplyFileChangesAsync(file, name, realName, uri, physicalPath);
        file = await GetFileByIdAsync(file.Id);
        return file;
    }

    public async Task DeleteFileAsync(ulong id)
    {
        await _fileRepository.DeleteFileAsync(id);
    }

    public async Task<bool> DoesFileIdExistAsync(ulong id)
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
            var fileNameLength = Configs.Current.File.GeneratedFileNameLength;
            fileName = Utils.GenerateRandomString(fileNameLength) + extension;
        } while (await DoesFileNameExistAsync(fileName));

        return fileName;
    }
}
