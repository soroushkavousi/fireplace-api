using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using FireplaceApi.Infrastructure.Converters;
using FireplaceApi.Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using System.Diagnostics;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ILogger<FileRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _fireplaceApiContext;
        private readonly DbSet<FileEntity> _fileEntities;
        private readonly FileConverter _fileConverter;
        

        public FileRepository(ILogger<FileRepository> logger, IConfiguration configuration, 
            FireplaceApiContext fireplaceApiContext, FileConverter fileConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _fireplaceApiContext = fireplaceApiContext;
            _fileEntities = fireplaceApiContext.FileEntities;
            _fileConverter = fileConverter;
            
        }

        public async Task<List<File>> ListFilesAsync()
        {
            var sw = Stopwatch.StartNew();
            var fileEntities = await _fileEntities
                .AsNoTracking()
                .Include(
                )
                .ToListAsync();

            _logger.LogIOInformation(sw, "Database", null, new { fileEntities });
            return fileEntities.Select(e => _fileConverter.ConvertToModel(e)).ToList();
        }

        public async Task<File> GetFileByIdAsync(long id)
        {
            var sw = Stopwatch.StartNew();
            var fileEntity = await _fileEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                )
                .SingleOrDefaultAsync();

            _logger.LogIOInformation(sw, "Database", new { id }, new { fileEntity });
            return _fileConverter.ConvertToModel(fileEntity);
        }

        public async Task<File> CreateFileAsync(string name, string realName, Uri uri,
            string physicalPath)
        {
            var sw = Stopwatch.StartNew();
            var relativeUri = _fileConverter.GetRelativeUri(uri).ToString();
            var relativePhysicalPath = _fileConverter.GetRelativePhysicalPath(physicalPath);
            var fileEntity = new FileEntity(name, realName, relativeUri, relativePhysicalPath);
            _fileEntities.Add(fileEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();
            
            _logger.LogIOInformation(sw, "Database", new { name, realName, uri, physicalPath }, new { fileEntity });
            return _fileConverter.ConvertToModel(fileEntity);
        }

        public async Task<File> UpdateFileAsync(File file)
        {
            var sw = Stopwatch.StartNew();
            var fileEntity = _fileConverter.ConvertToEntity(file);
            _fileEntities.Update(fileEntity);
            try
            {
                await _fireplaceApiContext.SaveChangesAsync();
                _fireplaceApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the dbFile DbUpdateConcurrencyException. {fileEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }
            
            _logger.LogIOInformation(sw, "Database", new { file }, new { fileEntity });
            return _fileConverter.ConvertToModel(fileEntity);
        }

        public async Task DeleteFileAsync(long id)
        {
            var sw = Stopwatch.StartNew();
            var fileEntity = await _fileEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _fileEntities.Remove(fileEntity);
            await _fireplaceApiContext.SaveChangesAsync();
            _fireplaceApiContext.DetachAllEntries();
        
            _logger.LogIOInformation(sw, "Database", new { id }, new { fileEntity });
        }

        public async Task<bool> DoesFileIdExistAsync(long id)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _fileEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();
        
            _logger.LogIOInformation(sw, "Database", new { id }, new { doesExist });
            return doesExist;
        }

        public async Task<bool> DoesFileNameExistAsync(string name)
        {
            var sw = Stopwatch.StartNew();
            var doesExist = await _fileEntities
                .AsNoTracking()
                .Where(e => e.Name == name)
                .AnyAsync();

            _logger.LogIOInformation(sw, "Database", new { name }, new { doesExist });
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
}
