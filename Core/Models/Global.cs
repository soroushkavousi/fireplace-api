using FireplaceApi.Core.Enums;
using FireplaceApi.Core.ValueObjects;
using System;

namespace FireplaceApi.Core.Models
{
    public class Global : BaseModel
    {
        public EnvironmentName EnvironmentName { get; set; }
        public GlobalValues Values { get; set; }

        public Global(ulong id, EnvironmentName environmentName,
            GlobalValues values, DateTime creationDate, DateTime? modifiedDate = null)
            : base(id, creationDate, modifiedDate)
        {
            EnvironmentName = environmentName;
            Values = values ?? throw new ArgumentNullException(nameof(values));
        }

        public Global PureCopy() => new Global(Id, EnvironmentName,
            Values, CreationDate, ModifiedDate);

        public void RemoveLoopReferencing()
        {

        }
    }
}
