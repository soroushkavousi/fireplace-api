namespace FireplaceApi.Domain.Common;

public class BaseModel
{
    public ulong Id { get; set; }
    public DateTime CreationDate { get; set; }
    public DateTime? ModifiedDate { get; set; }

    protected BaseModel() { }

    public BaseModel(ulong id, DateTime creationDate, DateTime? modifiedDate = null)
        : this()
    {
        Id = id;
        CreationDate = creationDate;
        ModifiedDate = modifiedDate;
    }
}
