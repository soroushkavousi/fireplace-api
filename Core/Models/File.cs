using System;

namespace FireplaceApi.Core.Models
{
    public class File : BaseModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string RealName { get; set; }
        public Uri Uri { get; set; }
        public string PhysicalPath { get; set; }
        //[JsonIgnore]
        //public IFormFile FormFile {get;set;}

        public File(long id, string name, string realName,
            Uri uri, string physicalPath, DateTime creationDate,
            DateTime? modifiedDate = null) : base(creationDate, modifiedDate)
        {
            Id = id;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            RealName = realName ?? throw new ArgumentNullException(nameof(realName));
            Uri = uri ?? throw new ArgumentNullException(nameof(uri));
            PhysicalPath = physicalPath ?? throw new ArgumentNullException(nameof(physicalPath));
        }

        public File PureCopy() => new File(Id, Name, RealName,
            Uri, PhysicalPath, CreationDate, ModifiedDate);

        public void RemoveLoopReferencing()
        {

        }
    }
}
