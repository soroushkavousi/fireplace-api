using FireplaceApi.Application.Enums;
using FireplaceApi.Application.Identifiers;
using FireplaceApi.Application.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Application.Interfaces;

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
