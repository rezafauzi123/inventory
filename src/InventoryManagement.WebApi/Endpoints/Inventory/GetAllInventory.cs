using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.Inventory.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;

namespace InventoryManagement.WebApi.Endpoints.Inventory;

public class GetAllInventory : BaseEndpoint<GetAllInventoryRequest, List<InventoryResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllInventory(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    //[Authorize]
    //[RequiredScope(typeof(InventoryScope), typeof(InventoryScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get Inventories",
        Description = "",
        OperationId = "Inventory.GetAllInventory",
        Tags = new[] { "Inventory" })
    ]
    [ProducesResponseType(typeof(List<InventoryResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<List<InventoryResponse>>> HandleAsync([FromQuery] GetAllInventoryRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<Domain.Entities.Inventory>()
            .Include(e => e.Book)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
        {
            queryable = queryable.Where(e => EF.Functions.Like(e.Book!.Title, $"%{request.Search}%"));
        }

        var data = await queryable
            .Select(e => new InventoryResponse
            {
                Id = e.Id,
                Stock = e.Stock,
                BookCode = e.BookCode,
                BookTitle = e.BookTitle,
                Author = e.Book!.Author!.Name,
                Publisher = e.Book.Publisher!.Name,
                Category = e.Book.Category!.Name,                
                Year = e.Book.Year,
                PublishedDate = e.Book.PublishedDate,
                Isbn = e.Book.Isbn,
                Price = e.Book.Price,
                Language = e.Book.Language,
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}