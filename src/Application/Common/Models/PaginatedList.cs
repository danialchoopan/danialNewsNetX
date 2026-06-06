namespace danialNewsNetX.Application.Common.Models;

public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public string? NextCursor { get; }

    public PaginatedList(IReadOnlyCollection<T> items, string? nextCursor)
    {
        Items = items;
        NextCursor = nextCursor;
    }
}
