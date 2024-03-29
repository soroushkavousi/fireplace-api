﻿using FireplaceApi.Domain.Enums;
using FireplaceApi.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FireplaceApi.Domain.Interfaces;

public interface ICommentRepository
{
    public Task<List<Comment>> ListPostCommentsAsync(ulong postId,
        SortType sort, User requestingUser = null);
    public Task<List<Comment>> ListChildCommentAsync(ulong id, SortType sort,
        User requestingUser = null);
    public Task<List<Comment>> ListCommentsByIdsAsync(
        List<ulong> Ids, SortType sort, User requestingUser = null);
    public Task<List<Comment>> ListSelfCommentsAsync(User author,
        SortType sort);
    public Task<Comment> GetCommentByIdAsync(ulong id,
        bool includeAuthor = false, bool includePost = false,
        User requestingUser = null);
    public Task<Comment> CreateCommentAsync(ulong id,
        ulong authorUserId, string authorUsername, ulong postId,
        string content, ulong? parentCommentId = null);
    public Task<Comment> UpdateCommentAsync(Comment comment);
    public Task DeleteCommentByIdAsync(ulong id);
    public Task<bool> DoesCommentIdExistAsync(ulong id);
}
