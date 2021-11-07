using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IErrorRepository
    {
        public Task<List<Error>> ListErrorsAsync();
        public Task<Error> GetErrorByNameAsync(ErrorName name);
        public Task<Error> GetErrorByCodeAsync(int code);
        public Task<Error> CreateErrorAsync(ulong id, ErrorName name,
            int code, string clientMessage, int httpStatusCode);
        public Task<Error> UpdateErrorAsync(Error error);
        public Task DeleteErrorAsync(int code);
        public Task<bool> DoesErrorNameExistAsync(ErrorName name);
        public Task<bool> DoesErrorCodeExistAsync(int code);
        public Task<bool> DoesErrorIdExistAsync(ulong id);
    }
}
