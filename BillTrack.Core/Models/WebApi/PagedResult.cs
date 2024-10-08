namespace BillTrack.Core.Models.WebApi;

public class PagedResult<T>
{
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public required List<T> Items { get; set; }
}