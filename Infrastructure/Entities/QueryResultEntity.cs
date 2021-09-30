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
            Pointer = pointer ?? throw new ArgumentNullException(nameof(pointer));
            LastStart = lastStart;
            LastEnd = lastEnd;
            LastLimit = lastLimit;
            LastPage = lastPage;
            ReferenceEntityIds = referenceEntityIds ?? throw new ArgumentNullException(nameof(referenceEntityIds));
            Id = id;
        }
    }

    public class QueryResultEntityConfiguration : IEntityTypeConfiguration<QueryResultEntity>
    {
        public void Configure(EntityTypeBuilder<QueryResultEntity> modelBuilder)
        {
            // p => principal / d => dependent

        }
    }
}
