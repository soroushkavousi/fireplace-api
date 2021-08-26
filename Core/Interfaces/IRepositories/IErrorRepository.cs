using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Enums;

namespace FireplaceApi.Core.Interfaces.IRepositories
{
    public interface IErrorRepository
    {
        public Task<List<Error>> ListErrorsAsync();
        public Task<Error> GetErrorByNameAsync(ErrorName name);
        public Task<Error> GetErrorByCodeAsync(int code);
        public Task<Error> CreateErrorAsync(ErrorName name, int code, string clientMessage,
            int httpStatusCode);
        public Task<Error> UpdateErrorAsync(Error error);
        public Task DeleteErrorAsync(int code);
        public Task<bool> DoesErrorNameExistAsync(ErrorName name);
        public Task<bool> DoesErrorCodeExistAsync(int code);
    }
}
