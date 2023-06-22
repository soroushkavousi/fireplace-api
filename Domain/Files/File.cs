using FireplaceApi.Domain.Common;

namespace FireplaceApi.Domain.Files;

public class File : BaseModel
{
    public string Name { get; set; }
    public string RealName { get; set; }
    public Uri Uri { get; set; }
    public string PhysicalPath { get; set; }
    //[JsonIgnore]
    //public IFormFile FormFile {get;set;}

    public File(ulong id, string name, string realName,
        Uri uri, string physicalPath, DateTime creationDate,
        DateTime? modifiedDate = null) : base(id, creationDate, modifiedDate)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        RealName = realName ?? throw new ArgumentNullException(nameof(realName));
        Uri = uri ?? throw new ArgumentNullException(nameof(uri));
        PhysicalPath = physicalPath ?? throw new ArgumentNullException(nameof(physicalPath));
    }

    public File PureCopy() => new(Id, Name, RealName,
        Uri, PhysicalPath, CreationDate, ModifiedDate);

    public void RemoveLoopReferencing()
    {

    }
}
