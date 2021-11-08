using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FireplaceApi.Core.Models
{
    public class QueryResult : BaseModel
    {
        public string Pointer { get; set; }
        public int LastStart { get; set; }
        public int LastEnd { get; set; }
        public int LastLimit { get; set; }
        public int LastPage { get; set; }
        [JsonIgnore]
        public List<ulong> ReferenceIds { get; set; }

        public QueryResult(ulong id, string pointer, int lastStart,
            int lastEnd, int lastLimit, int lastPage, List<ulong> referenceIds,
            DateTime creationDate, DateTime? modifiedDate = null)
            : base(id, creationDate, modifiedDate)
        {
            Pointer = pointer ?? throw new ArgumentNullException(nameof(pointer));
            LastStart = lastStart;
            LastEnd = lastEnd;
            LastLimit = lastLimit;
            LastPage = lastPage;
            ReferenceIds = referenceIds ?? throw new ArgumentNullException(nameof(referenceIds));
        }

        public QueryResult PureCopy() => new QueryResult(Id, Pointer,
            LastStart, LastEnd, LastLimit, LastPage, ReferenceIds,
            CreationDate, ModifiedDate);
    }
}
