using FireplaceApi.Application.Models;
using System.Collections.Generic;
using System.Linq;

namespace FireplaceApi.Application.ValueObjects;

public class QueryResult<T> where T : BaseModel
{
    public List<T> Items { get; set; }
    public List<ulong> MoreItemIds { get; set; }

    private QueryResult() { }

    public QueryResult(List<T> items)
    {
        if (items == null)
            return;

        Items = items
            .Take(Configs.Current.QueryResult.ViewLimit)
            .ToList();

        MoreItemIds = items
            .Skip(Configs.Current.QueryResult.ViewLimit)
            .Select(c => c.Id)
            .ToList();
    }

    public QueryResult(List<T> items, List<ulong> moreItemIds)
    {
        Items = items;
        MoreItemIds = moreItemIds;
    }
}
