using System;
using System.Collections.Generic;

namespace FireplaceApi.Core.Models
{
    public class Comment : BaseModel
    {
        public long Id { get; set; }
        public long AuthorId { get; set; }
        public string AuthorUsername { get; set; }
        public long PostId { get; set; }
        public int Vote { get; set; }
        public string Content { get; set; }
        public List<long> ParentCommentIds { get; set; }
        public User Author { get; set; }
        public Post Post { get; set; }
        public List<Comment> ChildComments { get; set; }

        public Comment(long id, long authorId, string authorUsername,
            long postId, int vote, string content,
            DateTime creationDate, List<long> parentCommentIds = null,
            DateTime? modifiedDate = null, User author = null,
            Post post = null, List<Comment> childComments = null)
            : base(creationDate, modifiedDate)
        {
            Id = id;
            AuthorId = authorId;
            AuthorUsername = authorUsername ?? throw new ArgumentNullException(nameof(authorUsername));
            PostId = postId;
            Vote = vote;
            Content = content ?? throw new ArgumentNullException(nameof(content));
            ParentCommentIds = parentCommentIds;
            Author = author;
            Post = post;
            ChildComments = childComments;
        }

        public Comment PureCopy() => new Comment(Id, AuthorId,
            AuthorUsername, PostId, Vote,
            Content, CreationDate, ParentCommentIds,
            ModifiedDate, childComments: ChildComments);
    }
}
