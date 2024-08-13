using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.Author.Requests;
using InventoryManagement.WebApi.Endpoints.Author.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.WebApi.Endpoints.Author;

public class GetAllAuthor : BaseEndpoint<GetAllAuthorRequest, List<AuthorResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllAuthor(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    //[Authorize]
    //[RequiredScope(typeof(AuthorScope), typeof(AuthorScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get Authors",
        Description = "",
        OperationId = "Author.GetAllAuthor",
        Tags = new[] { "Author" })
    ]
    [ProducesResponseType(typeof(List<AuthorResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<List<AuthorResponse>>> HandleAsync([FromQuery] GetAllAuthorRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<InventoryManagement.Domain.Entities.Author>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
            queryable = queryable.Where(e => EF.Functions.Like(e.Name, $"%{request.Search}%"));

        var data = await queryable
            .Select(e => new AuthorResponse
            {
                Id = e.Id,
                Name = e.Name,
                Biography = e.Biography,
                Image = e.Image,
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}