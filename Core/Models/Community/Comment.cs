using System;
using System.Collections.Generic;

namespace FireplaceApi.Core.Models
{
    public class Comment : BaseModel
    {
        public ulong AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public ulong PostId { get; set; }
        public int Vote { get; set; }
        public int RequestingUserVote { get; set; }
        public string Content { get; set; }
        public List<ulong> ParentCommentIds { get; set; }
        public User Author { get; set; }
        public Post Post { get; set; }
        public List<Comment> ChildComments { get; set; }

        public Comment(ulong id, ulong authorId, string authorUsername,
            ulong postId, int vote, int requestingUserVote, string content,
            DateTime creationDate, List<ulong> parentCommentIds = null,
            DateTime? modifiedDate = null, User author = null,
            Post post = null, List<Comment> childComments = null)
            : base(id, creationDate, modifiedDate)
        {
            AuthorId = authorId;
            AuthorUsername = authorUsername ?? throw new ArgumentNullException(nameof(authorUsername));
            PostId = postId;
            Vote = vote;
            RequestingUserVote = requestingUserVote;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            ParentCommentIds = parentCommentIds;
            Author = author;
            Post = post;
            ChildComments = childComments;
        }

        public Comment PureCopy() => new Comment(Id, AuthorId,
            AuthorUsername, PostId, Vote, RequestingUserVote,
            Content, CreationDate, ParentCommentIds,
            ModifiedDate, childComments: ChildComments);
    }
}
