namespace FireplaceApi.Domain.Common;

public class ExpirableCollection<T> where T : ExpirableData
{
    private List<T> _items = new();
    public TimeSpan LifeSpan { get; set; }
    public List<T> Items => GetNotExpiredItems();

    public ExpirableCollection(TimeSpan lifeSpan)
    {
        LifeSpan = lifeSpan;
    }

    public List<T> GetNotExpiredItems()
    {
        var currentDate = DateTime.UtcNow;
        _items = _items.Where(item => !item.IsExpired(LifeSpan)).ToList();
        return _items;
    }
}

public class ExpirableData
{
    public DateTime CreationDate { get; set; }

    public ExpirableData()
    {
        CreationDate = DateTime.UtcNow;
    }

    public bool IsExpired(TimeSpan lifeSpan)
    {
        var expirationDate = CreationDate.Add(lifeSpan);
        return expirationDate < DateTime.UtcNow;
    }
}

public class ExpirableData<T> : ExpirableData
{
    public T Data { get; set; }

    public ExpirableData(T data) : base()
    {
        Data = data;
    }
}
