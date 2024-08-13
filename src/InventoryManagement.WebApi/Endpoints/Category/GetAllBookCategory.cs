using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.Category.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace InventoryManagement.WebApi.Endpoints.Category;

public class GetAllCategory : BaseEndpoint<GetAllCategoryRequest, List<CategoryResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllCategory(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    //[Authorize]
    //[RequiredScope(typeof(CategoryScope), typeof(CategoryScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get  Categories",
        Description = "",
        OperationId = "Category.GetAllCategory",
        Tags = new[] { "Category" })
    ]
    [ProducesResponseType(typeof(List<CategoryResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<List<CategoryResponse>>> HandleAsync([FromQuery] GetAllCategoryRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Domain.Entities.Category>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
            queryable = queryable.Where(e => EF.Functions.Like(e.Name, $"%{request.Search}%"));

        var data = await queryable
            .Select(e => new CategoryResponse
            {
                Id = e.Id,
                Name = e.Name
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}