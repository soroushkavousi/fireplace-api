using FireplaceApi.Core.Enums;
using FireplaceApi.Core.Extensions;
using FireplaceApi.Core.Interfaces;
using FireplaceApi.Core.Models;
using FireplaceApi.Core.ValueObjects;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Operators
{
    public class CommentOperator
    {
        private readonly ILogger<CommentOperator> _logger;
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;
        private readonly ICommentRepository _commentRepository;
        private readonly PageOperator _pageOperator;
        private readonly UserOperator _userOperator;
        private readonly PostOperator _postOperator;

        public CommentOperator(ILogger<CommentOperator> logger,
            IConfiguration configuration, IServiceProvider serviceProvider,
            ICommentRepository commentRepository,
            PageOperator pageOperator, UserOperator userOperator,
            PostOperator postOperator)
        {
            _logger = logger;
            _configuration = configuration;
            _serviceProvider = serviceProvider;
            _commentRepository = commentRepository;
            _pageOperator = pageOperator;
            _userOperator = userOperator;
            _postOperator = postOperator;
        }

        public async Task<Page<Comment>> ListSelfCommentsAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, SortType? sort)
        {
            Page<Comment> resultPage = default;
            if (string.IsNullOrWhiteSpace(paginationInputParameters.Pointer))
            {
                var commentIds = await _commentRepository.ListSelfCommentIdsAsync(
                    requesterUser.Id, sort);
                resultPage = await _pageOperator.CreatePageWithoutPointerAsync(
                    ModelName.COMMENT, paginationInputParameters, commentIds,
                    _commentRepository.ListCommentsAsync);
            }
            else
            {
                resultPage = await _pageOperator.CreatePageWithPointerAsync(
                    ModelName.COMMENT, paginationInputParameters,
                    _commentRepository.ListCommentsAsync);
            }
            return resultPage;
        }

        public async Task<Page<Comment>> ListPostCommentsAsync(User requesterUser,
            PaginationInputParameters paginationInputParameters, long postId,
            SortType? sort)
        {
            Page<Comment> resultPage = default;
            if (string.IsNullOrWhiteSpace(paginationInputParameters.Pointer))
            {
                var commentIds = await _commentRepository.ListPostCommentIdsAsync(
                    postId, sort);
                resultPage = await _pageOperator.CreatePageWithoutPointerAsync(
                    ModelName.COMMENT, paginationInputParameters, commentIds,
                    _commentRepository.ListCommentsAsync);
            }
            else
            {
                resultPage = await _pageOperator.CreatePageWithPointerAsync(
                    ModelName.COMMENT, paginationInputParameters,
                    _commentRepository.ListCommentsAsync);
            }
            var childComments = await _commentRepository.ListChildCommentsAsync(
                    postId, resultPage.ItemIds);
            SetChilds(resultPage.Items, childComments);
            return resultPage;
        }

        private void SetChilds(List<Comment> parentComments,
            List<Comment> childComments)
        {
            var childsPerParentId = childComments
                .GroupBy(cc => cc.ParentCommentIds.Last())
                .ToDictionary(g => g.Key, g => g.ToList());
            SetChilds(parentComments, childsPerParentId);
        }

        private void SetChilds(List<Comment> parentComments,
            Dictionary<long, List<Comment>> childsPerParentId)
        {
            if (parentComments == null || parentComments.Count == 0)
                return;
            foreach (var pc in parentComments)
            {
                pc.ChildComments = childsPerParentId.GetValueOrDefault(pc.Id);
                SetChilds(pc.ChildComments, childsPerParentId);
            }
        }

        public async Task<List<Comment>> ListChildCommentsAsync(User requesterUser,
            long parentCommentId)
        {
            var postId = (await GetCommentByIdAsync(parentCommentId, false, false)).PostId;
            var childComments = await _commentRepository.ListChildCommentsAsync(
                    postId, parentCommentId);
            var rootComments = childComments.Where(cc => cc.ParentCommentIds.Last() == parentCommentId).ToList();
            rootComments.ForEach(rc => childComments.Remove(rc));
            SetChilds(rootComments, childComments);
            return rootComments;
        }

        public async Task<Comment> GetCommentByIdAsync(long id,
            bool includeAuthor, bool includePost)
        {
            var comment = await _commentRepository.GetCommentByIdAsync(
                id, includeAuthor, includePost);
            if (comment == null)
                return comment;

            return comment;
        }

        public async Task<Comment> ReplyToPostAsync(User requesterUser,
            long postId, string content)
        {
            var comment = await _commentRepository
                .CreateCommentAsync(requesterUser.Id,
                    requesterUser.Username, postId, content,
                    new List<long>());
            return comment;
        }

        public async Task<Comment> ReplyToCommentAsync(User requesterUser,
            long commentId, string content)
        {
            var parentComment = await _commentRepository.GetCommentByIdAsync(commentId);
            var postId = parentComment.PostId;
            var parentCommentIds = parentComment.ParentCommentIds.CopyOrDefault();
            parentCommentIds.Add(parentComment.Id);
            var comment = await _commentRepository
                .CreateCommentAsync(requesterUser.Id, requesterUser.Username,
                    postId, content, parentCommentIds);
            return comment;
        }

        public async Task<Comment> PatchCommentByIdAsync(long id, string content)
        {
            var comment = await _commentRepository
                .GetCommentByIdAsync(id);
            comment = await ApplyCommentChangesAsync(comment, content);
            comment = await GetCommentByIdAsync(comment.Id,
                false, false);
            return comment;
        }

        public async Task DeleteCommentByIdAsync(long id)
        {
            await _commentRepository.DeleteCommentByIdAsync(id);
        }

        public async Task<bool> DoesCommentIdExistAsync(long id)
        {
            var commentIdExists = await _commentRepository
                .DoesCommentIdExistAsync(id);
            return commentIdExists;
        }

        public async Task<Comment> ApplyCommentChangesAsync(
            Comment comment, string content)
        {
            if (content != null)
            {
                comment.Content = content;
            }

            comment = await _commentRepository
                .UpdateCommentAsync(comment);
            return comment;
        }
    }
}
