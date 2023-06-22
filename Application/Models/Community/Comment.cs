using FireplaceApi.Application.Enums;
using System;
using System.Collections.Generic;

namespace FireplaceApi.Application.Models;

public class Comment : BaseModel
{
    public ulong AuthorId { get; set; }
    public string AuthorUsername { get; set; }
    public ulong PostId { get; set; }
    public ulong? ParentCommentId { get; set; }
    public int Vote { get; set; }
    public VoteType RequestingUserVote { get; set; }
    public string Content { get; set; }
    public User Author { get; set; }
    public Post Post { get; set; }
    public Comment ParentComment { get; set; }
    public List<Comment> ChildComments { get; set; }
    public List<ulong> MoreChildCommentIds { get; set; }

    public Comment(ulong id, ulong authorId, string authorUsername,
        ulong postId, int vote, VoteType requestingUserVote, string content,
        DateTime creationDate, ulong? parentCommentId = null,
        DateTime? modifiedDate = null, User author = null,
        Post post = null, Comment parentComment = null,
        List<Comment> childComments = null, List<ulong> moreChildCommentIds = null)
        : base(id, creationDate, modifiedDate)
    {
        AuthorId = authorId;
        AuthorUsername = authorUsername ?? throw new ArgumentNullException(nameof(authorUsername));
        PostId = postId;
        ParentCommentId = parentCommentId;
        Vote = vote;
        RequestingUserVote = requestingUserVote;
        Content = content ?? throw new ArgumentNullException(nameof(content));
        Author = author;
        Post = post;
        ParentComment = parentComment;
        ChildComments = childComments;
        MoreChildCommentIds = moreChildCommentIds;
    }

    public Comment PureCopy() => new(Id, AuthorId,
        AuthorUsername, PostId, Vote, RequestingUserVote,
        Content, CreationDate, ParentCommentId,
        ModifiedDate, childComments: ChildComments,
        moreChildCommentIds: MoreChildCommentIds);
}
