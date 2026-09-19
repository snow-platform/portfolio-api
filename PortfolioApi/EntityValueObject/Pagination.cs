namespace PortfolioApi.EntityValueObject;

public class Pagination
{
    public int Page { get; set; }
    public int Size { get; set; }
    public string? SortBy { get; set; }
    public string? SortDirection { get; set; }
    public string? Search { get; set; }
    public List<Filter>? Filters { get; set; }

    public int SanitizePage()
    {
        return Math.Max(Page, 1);
    }

    public int SanitizeSize(int minimum = 1, int maximum = 10)
    {
        return Math.Min(maximum, Math.Max(Size, minimum));
    }

    public string SanitizeSortBy(params string[] valid)
    {
        return SortBy is not (null or "") && valid.Contains(SortBy)
            ? SortBy
            : "id";
    }

    public string SanitizeSortDirection()
    {
        return SortDirection is "asc" or "desc"
            ? SortDirection
            : "asc";
    }

    public List<Filter>? SanitizeFilters(params string[] valid)
    {
        return Filters?.GroupBy(x => x.Name)
            .Select(x => x.First())
            .Where(f => valid.Contains(f.Name))
            .ToList();
    }
}