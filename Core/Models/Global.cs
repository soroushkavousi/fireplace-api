using FireplaceApi.Core.ValueObjects;
using System;

namespace FireplaceApi.Core.Models
{
    public class Global : BaseModel
    {
        public GlobalValues Values { get; set; }

        public Global(ulong id, GlobalValues values,
            DateTime creationDate, DateTime? modifiedDate = null)
            : base(id, creationDate, modifiedDate)
        {
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public Global PureCopy() => new Global(Id, Values, CreationDate, ModifiedDate);

        public void RemoveLoopReferencing()
        {

        }
    }
}
