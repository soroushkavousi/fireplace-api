using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IGlobalRepository
    {
        public Task<List<Global>> ListGlobalsAsync();
        public Task<Global> GetGlobalByIdAsync(ulong globalId);
        public Task<Global> CreateGlobalAsync(ulong id,
            GlobalValues globalValues);
        public Task<Global> UpdateGlobalAsync(Global global);
        public Task DeleteGlobalAsync(ulong globalId);
        public Task<bool> DoesGlobalIdExistAsync(ulong globalId);
    }
}
