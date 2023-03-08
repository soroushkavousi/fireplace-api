using FireplaceApi.Domain.Models;
using FireplaceApi.Infrastructure.Entities;
using Microsoft.Extensions.Logging;
using System;

namespace FireplaceApi.Infrastructure.Converters
{
    public class PostVoteConverter
    {
        private readonly ILogger<PostVoteConverter> _logger;
        private readonly IServiceProvider _serviceProvider;
        private readonly UserConverter _voterConverter;
        private readonly PostConverter _postConverter;

        public PostVoteConverter(ILogger<PostVoteConverter> logger,
            IServiceProvider serviceProvider, UserConverter voterConverter,
            PostConverter postConverter)
        {
            _logger = logger;
            _serviceProvider = serviceProvider;
            _voterConverter = voterConverter;
            _postConverter = postConverter;
        }

        // Entity

        public PostVoteEntity ConvertToEntity(PostVote postVote)
        {
            if (postVote == null)
                return null;

            UserEntity voterEntity = null;
            if (postVote.Voter != null)
                voterEntity = _voterConverter
                    .ConvertToEntity(postVote.Voter.PureCopy());

            PostEntity postEntity = null;
            if (postVote.Post != null)
                postEntity = _postConverter
                    .ConvertToEntity(postVote.Post.PureCopy());

            var postVoteEntity = new PostVoteEntity(postVote.Id,
                postVote.VoterId, postVote.VoterUsername, postVote.PostId,
                postVote.IsUp, postVote.CreationDate,
                postVote.ModifiedDate, voterEntity, postEntity);

            return postVoteEntity;
        }

        public PostVote ConvertToModel(PostVoteEntity postVoteEntity)
        {
            if (postVoteEntity == null)
                return null;

            User voter = null;
            if (postVoteEntity.VoterEntity != null)
                voter = _voterConverter.ConvertToModel(postVoteEntity.VoterEntity.PureCopy());

            Post post = null;
            if (postVoteEntity.PostEntity != null)
                post = _postConverter
                    .ConvertToModel(postVoteEntity.PostEntity.PureCopy());

            var postVote = new PostVote(postVoteEntity.Id,
                postVoteEntity.VoterEntityId, postVoteEntity.VoterEntityUsername,
                postVoteEntity.PostEntityId, postVoteEntity.IsUp,
                postVoteEntity.CreationDate, postVoteEntity.ModifiedDate,
                voter, post);

            return postVote;
        }
    }
}
