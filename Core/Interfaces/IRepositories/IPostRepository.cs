using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IPostRepository
    {
        public Task<List<Post>> ListPostsAsync(List<ulong> Ids,
            User requesterUser = null);
        public Task<List<Post>> ListPostsAsync(ulong? authorId,
            bool? self, bool? joined, ulong? communityId,
            string communityName, string search, SortType? sort,
            User requesterUser = null);
        public Task<List<ulong>> ListPostIdsAsync(ulong? authorId,
            bool? self, bool? joined, ulong? communityId,
            string communityName, string search, SortType? sort);
        public Task<Post> GetPostByIdAsync(ulong id, bool includeAuthor = false,
            bool includeCommunity = false, User requesterUser = null);
        public Task<Post> CreatePostAsync(ulong id, ulong authorUserId,
            string authorUsername, ulong communityId,
            string communityName, string content);
        public Task<Post> UpdatePostAsync(
            Post post);
        public Task DeletePostByIdAsync(ulong id);
        public Task<bool> DoesPostIdExistAsync(ulong id);

    }
}
