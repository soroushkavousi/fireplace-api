using FireplaceApi.Core.Enums;
using FireplaceApi.Core.ValueObjects;
using System;

namespace FireplaceApi.Core.Models
{
    public class Global : BaseModel
    {
        public GlobalId Id { get; set; }
        public GlobalValues Values { get; set; }

        public Global(GlobalId id, GlobalValues values,
            DateTime creationDate, DateTime? modifiedDate = null) : base(creationDate, modifiedDate)
        {
            Id = id;
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public Global PureCopy() => new Global(Id, Values, CreationDate, ModifiedDate);

        public void RemoveLoopReferencing()
        {

        }
    }
}
