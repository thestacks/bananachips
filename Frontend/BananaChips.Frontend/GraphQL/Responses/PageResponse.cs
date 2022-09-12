namespace BananaChips.Frontend.GraphQL.Responses;

public class PageResponse<T>
{
    public List<T> Items { get; set; }
    public int TotalCount { get; set; }
}