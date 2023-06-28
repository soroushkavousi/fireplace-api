using FireplaceApi.Domain.Communities;
using FireplaceApi.Domain.Posts;
using FireplaceApi.Domain.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Posts;

public interface IPostRepository
{
    public Task<List<Post>> ListCommunityPostsAsync(CommunityIdentifier communityIdentifier,
        PostSortType sort, ulong? userId = null);
    public Task<List<Post>> ListPostsAsync(string search, PostSortType sort, ulong? userId = null);
    public Task<List<Post>> ListPostsByIdsAsync(List<ulong> Ids, ulong? userId = null);
    public Task<List<Post>> ListSelfPostsAsync(ulong authorId, PostSortType sort);
    public Task<Post> GetPostByIdAsync(ulong id, bool includeAuthor = false,
        bool includeCommunity = false, ulong? userId = null);
    public Task<Post> CreatePostAsync(ulong authorUserId,
        Username authorUsername, ulong communityId,
        CommunityName communityName, string content);
    public Task<Post> UpdatePostAsync(Post post);
    public Task DeletePostByIdAsync(ulong id);
    public Task<bool> DoesPostIdExistAsync(ulong id);

}
