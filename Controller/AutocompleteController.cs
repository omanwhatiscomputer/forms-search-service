using System;

using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.RequestHelpers;

namespace SearchService.Controller;

[ApiController]
[Route("api/autocomplete")]
public class AutocompleteController : ControllerBase
{
    public readonly int PageSize = 5;
    public readonly int PageNumber = 1;

    [HttpGet("users")]
    public async Task<ActionResult<List<User>>> PartialSearchUsers([FromQuery] SearchParams searchParams)
    {


        //--------------------
        var query = DB.PagedSearch<User>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(u => u.NormalizedName.Contains(searchParams.SearchTerm) || u.Email.Contains(searchParams.SearchTerm)).Sort(x => x.Ascending(a => a.NormalizedName));
        }

        query.PageNumber(PageNumber);
        query.PageSize(PageSize);

        var result = await query.ExecuteAsync();

        return Ok(result.Results);

    }

    [HttpGet("forms")]
    public async Task<ActionResult<List<Form>>> PartialSearchForms([FromQuery] SearchParams searchParams)
    {


        //--------------------
        var query = DB.PagedSearch<Form>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(f => f.Title.Contains(searchParams.SearchTerm) || f.Description.Contains(searchParams.SearchTerm)).Sort(x => x.Ascending(a => a.Title));
        }

        query.PageNumber(PageNumber);
        query.PageSize(PageSize);

        var result = await query.ExecuteAsync();

        return Ok(result.Results);

    }

}
