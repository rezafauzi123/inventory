using InventoryManagement.Shared.Abstractions.Databases;
using InventoryManagement.WebApi.Common;
using InventoryManagement.WebApi.Contracts.Responses;
using InventoryManagement.WebApi.Endpoints.Publisher.Requests;
using InventoryManagement.WebApi.Endpoints.Publisher.Scopes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;
using InventoryManagement.Domain.Entities;

namespace InventoryManagement.WebApi.Endpoints.Publisher;

public class GetAllPublisher : BaseEndpoint<GetAllPublisherRequest, List<PublisherResponse>>
{
    private readonly IDbContext _dbContext;

    public GetAllPublisher(IDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    //[Authorize]
    //[RequiredScope(typeof(PublisherScope), typeof(PublisherScopeReadOnly))]
    [SwaggerOperation(
        Summary = "Get Publishers",
        Description = "",
        OperationId = "Publisher.GetAllPublisher",
        Tags = new[] { "Publisher" })
    ]
    [ProducesResponseType(typeof(List<PublisherResponse>), StatusCodes.Status200OK)]
    public override async Task<ActionResult<List<PublisherResponse>>> HandleAsync([FromQuery] GetAllPublisherRequest request,
        CancellationToken cancellationToken = new())
    {
        var queryable = _dbContext.Set<InventoryManagement.Domain.Entities.Publisher>().AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search) && request.Search.Length > 2)
            queryable = queryable.Where(e => EF.Functions.Like(e.Name, $"%{request.Search}%"));

        var data = await queryable
            .Select(e => new PublisherResponse
            {
                Id = e.Id,
                Name = e.Name,
                Phone = e.Phone,
                Email = e.Email,
                Address = e.Address
            })
            .ToListAsync(cancellationToken);

        return data;
    }
}