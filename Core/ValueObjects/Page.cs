using System;
using System.Collections.Generic;

namespace FireplaceApi.Core.ValueObjects
{
    public class PaginationInputParameters
    {
        public int? Limit { get; set; }
        public string Pointer { get; set; }
        public bool? Next { get; set; }
        public bool? Previous { get; set; }
        public int? Page { get; set; }
        public int? Offset { get; set; }

        public PaginationInputParameters(int? limit, string pointer, bool? next,
            bool? previous, int? page, int? offset)
        {
            Limit = limit;
            Pointer = pointer;
            Next = next;
            Previous = previous;
            Page = page;
            Offset = offset;
        }
    }

    public class Page<T>
    {
        public string QueryResultPointer { get; set; }
        public int? Number { get; set; }
        public int? Start { get; set; }
        public int? End { get; set; }
        public int? Limit { get; set; }
        public int TotalItemsCount { get; set; }
        public int TotalPagesCount { get; set; }
        public List<T> Items { get; set; }

        private Page() { }

        public Page(string queryResultPointer, int? number, int? start, int? end,
            int? limit, int totalItemsCount, int totalPagesCount, List<T> items)
        {
            QueryResultPointer = queryResultPointer;
            Number = number;
            Start = start;
            End = end;
            Limit = limit;
            TotalItemsCount = totalItemsCount;
            TotalPagesCount = totalPagesCount;
            Items = items ?? throw new ArgumentNullException(nameof(items));
        }
    }
}
