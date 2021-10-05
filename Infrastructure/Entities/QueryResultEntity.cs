using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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
        public List<long> ReferenceEntityIds { get; set; }
        public long? Id { get; set; }

        protected QueryResultEntity() : base() { }

        public QueryResultEntity(string pointer, int lastStart,
            int lastEnd, int lastLimit, int lastPage, List<long> referenceEntityIds,
            DateTime? creationDate = null, DateTime? modifiedDate = null, 
            long? id = null) : base(creationDate, modifiedDate)
        {
            FillParameters(pointer, lastStart, lastEnd, lastLimit, lastPage,
                referenceEntityIds, creationDate, modifiedDate, id);
        }

        public void FillParameters(string pointer, int lastStart,
            int lastEnd, int lastLimit, int lastPage, List<long> referenceEntityIds,
            DateTime? creationDate = null, DateTime? modifiedDate = null,
            long? id = null)
        {
            Pointer = pointer ?? throw new ArgumentNullException(nameof(pointer));
            LastStart = lastStart;
            LastEnd = lastEnd;
            LastLimit = lastLimit;
            LastPage = lastPage;
            ReferenceEntityIds = referenceEntityIds ?? throw new ArgumentNullException(nameof(referenceEntityIds));
            if (creationDate.HasValue)
                CreationDate = creationDate.Value;
            if (modifiedDate.HasValue)
                ModifiedDate = modifiedDate.Value;
            Id = id;
        }
    }

    public class CommunityQueryResultEntity : QueryResultEntity { }
    public class CommunityMembershipQueryResultEntity : QueryResultEntity { }

    //public class CommunityQueryResultEntityConfiguration : IEntityTypeConfiguration<CommunityQueryResultEntity>
    //{
    //    public void Configure(EntityTypeBuilder<CommunityQueryResultEntity> modelBuilder)
    //    {
    //        // p => principal / d => dependent

    //        modelBuilder.ToTable("CommunityQueryResultEntities");
    //    }
    //}

    //public class CommunityMembershipQueryResultEntityConfiguration : IEntityTypeConfiguration<CommunityMembershipQueryResultEntity>
    //{
    //    public void Configure(EntityTypeBuilder<CommunityMembershipQueryResultEntity> modelBuilder)
    //    {
    //        // p => principal / d => dependent

    //        modelBuilder.ToTable("CommunityMembershipQueryResultEntities");
    //    }
    //}
}
