using System;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Entities;
using SearchService.RequestHelpers;

namespace SearchService.Controller;

[ApiController]
[Route("api/search")]
public class SearchController : ControllerBase
{
    public readonly int PageSize = 6;
    public readonly int PageNumber = 1;

    [HttpGet("users")]
    public async Task<ActionResult<List<User>>> SearchUsers([FromQuery] SearchParams searchParams)
    {


        //--------------------
        var query = DB.PagedSearch<User>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query.PageNumber(PageNumber);
        query.PageSize(PageSize);

        var result = await query.ExecuteAsync();

        return Ok(result.Results);

    }

    [HttpGet("forms")]
    public async Task<ActionResult<List<Form>>> SearchForms([FromQuery] SearchParams searchParams)
    {


        //--------------------
        var query = DB.PagedSearch<Form>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query.PageNumber(PageNumber);
        query.PageSize(PageSize);

        var result = await query.ExecuteAsync();

        return Ok(result.Results);

    }

}
