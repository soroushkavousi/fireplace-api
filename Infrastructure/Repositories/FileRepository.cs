using FireplaceApi.Domain.Exceptions;
using FireplaceApi.Domain.Extensions;
using FireplaceApi.Domain.Interfaces;
using FireplaceApi.Domain.Models;
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

public class FileRepository : IFileRepository
{
    private readonly ILogger<FileRepository> _logger;
    private readonly ProjectDbContext _dbContext;
    private readonly DbSet<FileEntity> _fileEntities;
    private readonly FileConverter _fileConverter;


    public FileRepository(ILogger<FileRepository> logger,
        ProjectDbContext dbContext, FileConverter fileConverter)
    {
        _logger = logger;
        _dbContext = dbContext;
        _fileEntities = dbContext.FileEntities;
        _fileConverter = fileConverter;
    }

    public async Task<List<File>> ListFilesAsync()
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: null);
        var sw = Stopwatch.StartNew();
        var fileEntities = await _fileEntities
            .AsNoTracking()
            .Include(
            )
            .ToListAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT",
            parameters: new { fileEntities = fileEntities.Select(e => e.Id) });
        return fileEntities.Select(_fileConverter.ConvertToModel).ToList();
    }

    public async Task<File> GetFileByIdAsync(ulong id)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var fileEntity = await _fileEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .Include(
            )
            .SingleOrDefaultAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { fileEntity });
        return _fileConverter.ConvertToModel(fileEntity);
    }

    public async Task<File> CreateFileAsync(ulong id, string name, string realName,
        Uri uri, string physicalPath)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT",
            parameters: new { id, name, realName, uri, physicalPath });
        var sw = Stopwatch.StartNew();
        var relativeUri = _fileConverter.GetRelativeUri(uri).ToString();
        var relativePhysicalPath = _fileConverter.GetRelativePhysicalPath(physicalPath);
        var fileEntity = new FileEntity(id, name, realName, relativeUri, relativePhysicalPath);
        _fileEntities.Add(fileEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { fileEntity });
        return _fileConverter.ConvertToModel(fileEntity);
    }

    public async Task<File> UpdateFileAsync(File file)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { file });
        var sw = Stopwatch.StartNew();
        var fileEntity = _fileConverter.ConvertToEntity(file);
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

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { fileEntity });
        return _fileConverter.ConvertToModel(fileEntity);
    }

    public async Task DeleteFileAsync(ulong id)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var fileEntity = await _fileEntities
            .Where(e => e.Id == id)
            .SingleOrDefaultAsync();

        _fileEntities.Remove(fileEntity);
        await _dbContext.SaveChangesAsync();
        _dbContext.DetachAllEntries();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { fileEntity });
    }

    public async Task<bool> DoesFileIdExistAsync(ulong id)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { id });
        var sw = Stopwatch.StartNew();
        var doesExist = await _fileEntities
            .AsNoTracking()
            .Where(e => e.Id == id)
            .AnyAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
        return doesExist;
    }

    public async Task<bool> DoesFileNameExistAsync(string name)
    {
        _logger.LogAppInformation(title: "DATABASE_INPUT", parameters: new { name });
        var sw = Stopwatch.StartNew();
        var doesExist = await _fileEntities
            .AsNoTracking()
            .Where(e => e.Name == name)
            .AnyAsync();

        _logger.LogAppInformation(sw: sw, title: "DATABASE_OUTPUT", parameters: new { doesExist });
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
