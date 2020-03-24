using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using GamingCommunityApi.Core.Models;
using GamingCommunityApi.Core.Enums;
using GamingCommunityApi.Core.ValueObjects;

namespace GamingCommunityApi.Core.Interfaces.IRepositories
{
    public interface IGlobalRepository
    {
        public Task<List<Global>> ListGlobalsAsync();
        public Task<Global> GetGlobalByIdAsync(GlobalId globalId);
        public Task<Global> CreateGlobalAsync(GlobalId globalId, GlobalValues globalValues);
        public Task<Global> UpdateGlobalAsync(Global global);
        public Task DeleteGlobalAsync(GlobalId globalId);
        public Task<bool> DoesGlobalIdExist(GlobalId globalId);
    }
}
