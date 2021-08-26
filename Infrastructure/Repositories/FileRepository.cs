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
using FireplaceApi.Core.Interfaces.IRepositories;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Exceptions;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;

namespace FireplaceApi.Infrastructure.Repositories
{
    public class FileRepository : IFileRepository
    {
        private readonly ILogger<FileRepository> _logger;
        private readonly IConfiguration _configuration;
        private readonly FireplaceApiContext _gamingCommunityApiContext;
        private readonly DbSet<FileEntity> _fileEntities;
        private readonly FileConverter _fileConverter;
        

        public FileRepository(ILogger<FileRepository> logger, IConfiguration configuration, 
            FireplaceApiContext gamingCommunityApiContext, FileConverter fileConverter)
        {
            _logger = logger;
            _configuration = configuration;
            _gamingCommunityApiContext = gamingCommunityApiContext;
            _fileEntities = gamingCommunityApiContext.FileEntities;
            _fileConverter = fileConverter;
            
        }

        public async Task<List<File>> ListFilesAsync()
        {
            var fileEntities = await _fileEntities
                .AsNoTracking()
                .Include(
                )
                .ToListAsync();

            return fileEntities.Select(e => _fileConverter.ConvertToModel(e)).ToList();
        }

        public async Task<File> GetFileByIdAsync(long id)
        {
            var fileEntity = await _fileEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .Include(
                )
                .SingleOrDefaultAsync();

            return _fileConverter.ConvertToModel(fileEntity);
        }

        public async Task<File> CreateFileAsync(string name, string realName, Uri uri,
            string physicalPath)
        {
            var relativeUri = _fileConverter.GetRelativeUri(uri).ToString();
            var relativePhysicalPath = _fileConverter.GetRelativePhysicalPath(physicalPath);
            var fileEntity = new FileEntity(name, realName, relativeUri, relativePhysicalPath);
            _fileEntities.Add(fileEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
            return _fileConverter.ConvertToModel(fileEntity);
        }

        public async Task<File> UpdateFileAsync(File file)
        {
            var fileEntity = _fileConverter.ConvertToEntity(file);
            _fileEntities.Update(fileEntity);
            try
            {
                await _gamingCommunityApiContext.SaveChangesAsync();
                _gamingCommunityApiContext.DetachAllEntries();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                var serverMessage = $"Can't update the dbFile DbUpdateConcurrencyException. {fileEntity.ToJson()}";
                throw new ApiException(ErrorName.INTERNAL_SERVER, serverMessage, systemException: ex);
            }
            return _fileConverter.ConvertToModel(fileEntity);
        }

        public async Task DeleteFileAsync(long id)
        {
            var fileEntity = await _fileEntities
                .Where(e => e.Id == id)
                .SingleOrDefaultAsync();

            _fileEntities.Remove(fileEntity);
            await _gamingCommunityApiContext.SaveChangesAsync();
            _gamingCommunityApiContext.DetachAllEntries();
        }

        public async Task<bool> DoesFileIdExistAsync(long id)
        {
            return await _fileEntities
                .AsNoTracking()
                .Where(e => e.Id == id)
                .AnyAsync();
        }

        public async Task<bool> DoesFileNameExistAsync(string name)
        {
            return await _fileEntities
                .AsNoTracking()
                .Where(e => e.Name == name)
                .AnyAsync();
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
