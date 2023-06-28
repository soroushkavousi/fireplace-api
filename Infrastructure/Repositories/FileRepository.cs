﻿using FireplaceApi.Application.Files;
using FireplaceApi.Domain.Errors;
using FireplaceApi.Domain.Files;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Infrastructure.Repositories;

public class FileRepository : IFileRepository, IRepository<IFileRepository>
{
    private readonly ILogger<FileRepository> _logger;
    private readonly ApiDbContext _dbContext;
    private readonly IIdGenerator _idGenerator;
    private readonly DbSet<FileEntity> _fileEntities;

    public FileRepository(ILogger<FileRepository> logger, ApiDbContext dbContext,
        IIdGenerator idGenerator)
    {
        _logger = logger;
        _dbContext = dbContext;
        _idGenerator = idGenerator;
        _fileEntities = dbContext.FileEntities;
    }

    public async Task<List<File>> ListFilesAsync()
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: null);
        var sw = Stopwatch.StartNew();
        var fileEntities = await _fileEntities
            .AsNoTracking()
            .Include(
            )
            .ToListAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { fileEntities = fileEntities.Select(e => e.Id) });
        return fileEntities.Select(FileConverter.ToModel).ToList();
    }

    public async Task<File> GetFileByIdAsync(ulong id)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var fileEntity = await _fileEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Include(
            )
            .SingleOrDefaultAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { fileEntity });
        return FileConverter.ToModel(fileEntity);
    }

    public async Task<File> CreateFileAsync(string name, string realName,
        Uri uri, string physicalPath)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT",
            parameters: new { name, realName, uri, physicalPath });
        var sw = Stopwatch.StartNew();
        var id = _idGenerator.GenerateNewId();
        var relativeUri = uri.ToRelativeUri().ToString();
        var relativePhysicalPath = physicalPath.ToRelativePhysicalPath();
        var fileEntity = new FileEntity(id, name, realName, relativeUri, relativePhysicalPath);
        _fileEntities.Add(fileEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { fileEntity });
        return FileConverter.ToModel(fileEntity);
    }

    public async Task<File> UpdateFileAsync(File file)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { file });
        var sw = Stopwatch.StartNew();
        var fileEntity = file.ToEntity();
        _fileEntities.Update(fileEntity);
        try
        {
            await _dbContext.SaveChangesAsync();
            _dbContext.DetachAllEntries();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new InternalServerException("Can't update the fileEntity DbUpdateConcurrencyException!",
                parameters: fileEntity, systemException: ex);
        }

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { fileEntity });
        return FileConverter.ToModel(fileEntity);
    }

    public async Task DeleteFileAsync(ulong id)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var fileEntity = await _fileEntities
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        _fileEntities.Remove(fileEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { fileEntity });
    }

    public async Task<bool> DoesFileIdExistAsync(ulong id)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var doesExist = await _fileEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .AnyAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }

    public async Task<bool> DoesFileNameExistAsync(string name)
    {
        _logger.LogServerInformation(title: "DATABASE_INPUT", parameters: new { name });
        var sw = Stopwatch.StartNew();
        var doesExist = await _fileEntities
            .AsNoTracking()
            .Where(e => e.Name == name)
            .AnyAsync();

        _logger.LogServerInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }
}

public static class FileRepositoryExtensions
{
    public static IQueryable<FileEntity> Include(
                [NotNull] this IQueryable<FileEntity> fileEntitiesQuery)
    {
        return fileEntitiesQuery;
    }
}
