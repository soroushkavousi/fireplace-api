using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Interfaces
{
    public interface IPostRepository
    {
        public Task<List<Post>> ListPostsAsync(List<long> Ids,
            User requesterUser = null);
        public Task<List<Post>> ListPostsAsync(long? authorId,
            bool? self, bool? joined, long? communityId,
            string communityName, string search, SortType? sort,
            User requesterUser = null);
        public Task<List<long>> ListPostIdsAsync(long? authorId,
            bool? self, bool? joined, long? communityId,
            string communityName, string search, SortType? sort);
        public Task<Post> GetPostByIdAsync(long id,
            bool includeAuthor = false, bool includeCommunity = false,
            User requesterUser = null);
        public Task<Post> CreatePostAsync(long authorUserId,
            string authorUsername, long communityId,
            string communityName, string content);
        public Task<Post> UpdatePostAsync(
            Post post);
        public Task DeletePostByIdAsync(long id);
        public Task<bool> DoesPostIdExistAsync(long id);

    }
}
