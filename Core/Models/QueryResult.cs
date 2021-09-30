using FireplaceApi.Core.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace FireplaceApi.Core.Models
{
    public class QueryResult : BaseModel
    {
        public long Id { get; set; }
        public string Pointer { get; set; }
        public int LastStart { get; set; }
        public int LastEnd { get; set; }
        public int LastLimit { get; set; }
        public int LastPage { get; set; }
        [JsonIgnore]
        public List<long> ReferenceIds { get; set; }

        public QueryResult(long id, string pointer, int lastStart,
            int lastEnd, int lastLimit, int lastPage, List<long> referenceIds, 
            DateTime creationDate,  DateTime? modifiedDate = null) 
            : base(creationDate, modifiedDate)
        {
            Id = id;
            Pointer = pointer ?? throw new ArgumentNullException(nameof(pointer));
            LastStart = lastStart;
            LastEnd = lastEnd;
            LastLimit = lastLimit;
            LastPage = lastPage;
            ReferenceIds = referenceIds ?? throw new ArgumentNullException(nameof(referenceIds)); ;
        }

        public QueryResult PureCopy() => new QueryResult(Id, Pointer,
            LastStart, LastEnd, LastLimit, LastPage, ReferenceIds, 
            CreationDate, ModifiedDate);
    }
}
