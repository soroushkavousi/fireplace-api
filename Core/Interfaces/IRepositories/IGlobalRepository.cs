using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.Enums;
using FireplaceApi.Core.ValueObjects;

namespace FireplaceApi.Core.Interfaces.IRepositories
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
