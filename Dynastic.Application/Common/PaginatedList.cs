using Mapster;
using Microsoft.EntityFrameworkCore;

namespace Dynastic.Application.Common;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public long TotalCount { get; }
    public int PageSize { get; set; }

    public PaginatedList(List<T> items, long count, int pageNumber, int pageSize)
    {
        PageSize = pageSize;
        PageNumber = pageNumber;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        TotalCount = count;
        Items = items;
    }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;

    public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        var count = await source.CountAsync();
        var items = await source.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        return new PaginatedList<T>(items, count, pageNumber, pageSize);
    }

    public PaginatedList<TR> AdaptItems<TR>()
    {
        return new PaginatedList<TR>(Items.Adapt<List<TR>>(), TotalCount, PageNumber, PageSize);
    }
}
