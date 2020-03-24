using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models;

namespace GamingCommunityApi.Core.Interfaces.IRepositories
{
    public interface IFileRepository
    {
        public Task<List<File>> ListFilesAsync();
        public Task<File> GetFileByIdAsync(long id);
        public Task<File> CreateFileAsync(string name, string realName, Uri uri,
            string physicalPath);
        public Task<File> UpdateFileAsync(File file);
        public Task DeleteFileAsync(long id);
        public Task<bool> DoesFileIdExistAsync(long id);
        public Task<bool> DoesFileNameExistAsync(string name);
    }
}
