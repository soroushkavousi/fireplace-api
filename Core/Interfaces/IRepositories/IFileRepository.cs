using FireplaceApi.Core.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
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
