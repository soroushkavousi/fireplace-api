using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IGlobalRepository
    {
        public Task<List<Global>> ListGlobalsAsync();
        public Task<Global> GetGlobalByIdAsync(GlobalId globalId);
        public Task<Global> CreateGlobalAsync(GlobalId globalId, GlobalValues globalValues);
        public Task<Global> UpdateGlobalAsync(Global global);
        public Task DeleteGlobalAsync(GlobalId globalId);
        public Task<bool> DoesGlobalIdExistAsync(GlobalId globalId);
    }
}
