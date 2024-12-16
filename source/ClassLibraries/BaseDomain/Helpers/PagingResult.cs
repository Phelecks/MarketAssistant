namespace BaseDomain.Helpers;

public class PagingResult<TDestination>
{
    public List<TDestination> data { get; set; }

    /// <summary>
    /// Paging information
    /// </summary>
    public PagingInformation pagingInformation { get; set; }
}

/// <summary>
/// Paging information
/// </summary>
public class PagingInformation
{
    /// <summary>
    /// Current page
    /// </summary>
    public int currentPage { get; set; }

    /// <summary>
    /// Total pages
    /// </summary>
    public int totalPages { get; set; }

    /// <summary>
    /// Page size
    /// </summary>
    public int pageSize { get; set; }

    /// <summary>
    /// Total count
    /// </summary>
    public int totalCount { get; set; }

    /// <summary>
    /// Has previous page
    /// </summary>
    public bool hasPrevious => currentPage > 1;

    /// <summary>
    /// Has next page
    /// </summary>
    public bool hasNext => currentPage < totalPages;
}

public class PagingLink
{
    public string text { get; set; }
    public int page { get; set; }
    public bool enabled { get; set; }
    public bool active { get; set; }

    public PagingLink(int page, bool enabled, string text)
    {
        this.page = page;
        this.enabled = enabled;
        this.text = text;
    }
}