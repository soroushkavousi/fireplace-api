using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FireplaceApi.Core.ValueObjects
{
    public class Page<T>
    {
        public int TotalItemsCount { get; set; }
        public Pagination Pagination { get; set; }
        public List<T> Items { get; set; }

        private Page() { }

        public Page(int totalItemsCount, List<T> items, Pagination pagination = null)
        {
            TotalItemsCount = totalItemsCount;
            Pagination = pagination;
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }
    }

    public class Pagination
    {
        public int TotalPagesCount { get; set; }
        public string Cursor { get; set; }

        private Pagination() { }

        public Pagination(int totalPagesCount, string cursor = null)
        {
            TotalPagesCount = totalPagesCount;
            Cursor = cursor;
        }
    }
}
