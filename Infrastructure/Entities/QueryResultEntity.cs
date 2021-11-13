using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FireplaceApi.Infrastructure.Entities
{
    [Index(nameof(Pointer), IsUnique = true)]
    public class QueryResultEntity : BaseEntity
    {
        public string Pointer { get; set; }
        public int LastStart { get; set; }
        public int LastEnd { get; set; }
        public int LastLimit { get; set; }
        public int LastPage { get; set; }
        [JsonIgnore]
        public List<decimal> ReferenceEntityIds { get; set; }

        protected QueryResultEntity() : base() { }

        public QueryResultEntity(ulong id, string pointer, int lastStart,
            int lastEnd, int lastLimit, int lastPage, List<decimal> referenceEntityIds,
            DateTime? creationDate = null, DateTime? modifiedDate = null)
            : base(id, creationDate, modifiedDate)
        {
            FillParameters(id, pointer, lastStart, lastEnd, lastLimit, lastPage,
                referenceEntityIds, creationDate, modifiedDate);
        }

        public void FillParameters(ulong id, string pointer, int lastStart,
            int lastEnd, int lastLimit, int lastPage, List<decimal> referenceEntityIds,
            DateTime? creationDate = null, DateTime? modifiedDate = null)
        {
            Id = id;
            Pointer = pointer ?? throw new ArgumentNullException(nameof(pointer));
            LastStart = lastStart;
            LastEnd = lastEnd;
            LastLimit = lastLimit;
            LastPage = lastPage;
            ReferenceEntityIds = referenceEntityIds;
            if (creationDate.HasValue)
                CreationDate = creationDate.Value;
            if (modifiedDate.HasValue)
                ModifiedDate = modifiedDate.Value;
        }
    }


    public class CommunityQueryResultEntity : QueryResultEntity { }
    public class CommunityMembershipQueryResultEntity : QueryResultEntity { }
    public class PostQueryResultEntity : QueryResultEntity { }
    public class CommentQueryResultEntity : QueryResultEntity { }

    public class CommunityQueryResultEntityConfiguration : IEntityTypeConfiguration<CommunityQueryResultEntity>
    {
        public void Configure(EntityTypeBuilder<CommunityQueryResultEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder.DoBaseConfiguration();
        }
    }
    public class CommunityMembershipQueryResultEntityConfiguration : IEntityTypeConfiguration<CommunityMembershipQueryResultEntity>
    {
        public void Configure(EntityTypeBuilder<CommunityMembershipQueryResultEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder.DoBaseConfiguration();
        }
    }
    public class PostQueryResultEntityConfiguration : IEntityTypeConfiguration<PostQueryResultEntity>
    {
        public void Configure(EntityTypeBuilder<PostQueryResultEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder.DoBaseConfiguration();
        }
    }
    public class CommentQueryResultEntityConfiguration : IEntityTypeConfiguration<CommentQueryResultEntity>
    {
        public void Configure(EntityTypeBuilder<CommentQueryResultEntity> modelBuilder)
        {
            // p => principal / d => dependent

            modelBuilder.DoBaseConfiguration();
        }
    }
}
