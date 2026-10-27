namespace Application.Common.Pagination;

public class PageRequest
{
    public const int MaxPageSize = 50;
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}