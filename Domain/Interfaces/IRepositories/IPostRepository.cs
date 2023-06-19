using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Identifiers;
using FireplaceApi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Interfaces;

public interface IPostRepository
{
    public Task<List<Post>> ListCommunityPostsAsync(CommunityIdentifier communityIdentifier,
        SortType sort, ulong? userId = null);
    public Task<List<Post>> ListPostsAsync(string search, SortType sort, ulong? userId = null);
    public Task<List<Post>> ListPostsByIdsAsync(List<ulong> Ids, ulong? userId = null);
    public Task<List<Post>> ListSelfPostsAsync(ulong authorId, SortType sort);
    public Task<Post> GetPostByIdAsync(ulong id, bool includeAuthor = false,
        bool includeCommunity = false, ulong? userId = null);
    public Task<Post> CreatePostAsync(ulong id, ulong authorUserId,
        string authorUsername, ulong communityId,
        string communityName, string content);
    public Task<Post> UpdatePostAsync(Post post);
    public Task DeletePostByIdAsync(ulong id);
    public Task<bool> DoesPostIdExistAsync(ulong id);

}
