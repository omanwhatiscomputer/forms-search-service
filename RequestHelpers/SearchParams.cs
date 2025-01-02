using System;

namespace SearchService.RequestHelpers;

public class SearchParams
{
    public string SearchTerm { get; set; } = string.Empty;
    public int PageSize { get; set; } = 4;
    public int PageNumber { get; set; } = 1;
}
